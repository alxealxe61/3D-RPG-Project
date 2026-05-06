using System.Collections.Generic;
using _01._Script.Data;
using _01._Script.Enemy.Boss_Enemy.Boss_Enemy_State;
using _01._Script.Enemy.Boss_Enemy.Boss_Enemy_State.CombatState;
using _01._Script.Enemy.Boss_Enemy.Boss_Enemy_State.CombatState.Pattern1;
using _01._Script.Enemy.Boss_Enemy.Boss_Enemy_State.CombatState.Pattern2;
using _01._Script.Enemy.Boss_Enemy.Boss_Enemy_State.CombatState.Pattern3;
using _01._Script.Enemy.Range_Enemy.Bullet;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _01._Script.Enemy.Boss_Enemy
{
    public class BossController : MonoBehaviour
    {
        public BossStats bossStats;
        
        [SerializeField] public Animator ani;
        
        [SerializeField] private DetectionRange detectionRange;
        [SerializeField] private AttackRange attackRange;
        
        public float MoveSpeed => bossStats.MoveSpeed;
        
        public Transform Target => detectionRange.detectedTarget;

        private readonly List<BossState> _patterns = new List<BossState>();
        
        public Transform firePoint;
        
        [Header("HitBoxes")]
        public LHitBox lHitBox;
        public RHitBox rHitBox;
        public PullHitBox pHitBox;
        
        public GameObject fireObject;
        
        private const float AttackDelay = 4.0f; 
        public bool isPreparingAttack;

        public float idleTimer;
        public BulletPool bulletPool;
        
        public AudioSource audioSource;
        
        #region 상태 머신 모음
        private BossStateMachine StateMachine { get; set; }
        
        public BossIdleState BossIdleState { get; private set; }
        private BossMoveState BossMoveState { get; set; }
        private BossStunState BossStunState { get; set; }
        private Pattern1Attack1 Pattern1Attack1 { get; set; }
        public Pattern1Attack2 Pattern1Attack2 { get; private set; }
        public Pattern1Attack3 Pattern1Attack3 { get; private set; }

        private Pattern2Attack1 Pattern2Attack1 { get; set; }
        public Pattern2Attack2 Pattern2Attack2 { get; private set; }
        private Pattern3Attack1 Pattern3Attack1 { get; set; }
        public Pattern3Attack2 Pattern3Attack2 { get; private set; }
        private BossDieState BossDieState { get; set; }
        
        #endregion

        private void Awake()
        {
            StateMachine = new BossStateMachine();

            BossIdleState = new BossIdleState(this, StateMachine, "BossIdle");
            BossMoveState = new BossMoveState(this, StateMachine, "BossMove");
            BossStunState = new BossStunState(this, StateMachine, "BossStun");
            Pattern1Attack1 = new Pattern1Attack1(this, StateMachine, "Pattern1Attack1");
            Pattern1Attack2 = new Pattern1Attack2(this, StateMachine, "Pattern1Attack2");
            Pattern1Attack3 = new Pattern1Attack3(this, StateMachine, "Pattern1Attack3");
            Pattern2Attack1 = new Pattern2Attack1(this, StateMachine, "Pattern2Attack1");
            Pattern2Attack2 = new Pattern2Attack2(this, StateMachine, "Pattern2Attack2");
            Pattern3Attack1 = new Pattern3Attack1(this, StateMachine, "Pattern3Attack1");
            Pattern3Attack2 = new Pattern3Attack2(this, StateMachine, "Pattern3Attack2");
            BossDieState = new BossDieState(this, StateMachine, "BossDie");

        }

        private void Start()
        {
            StateMachine.Initialize(BossIdleState);
            // 여기에 패턴들 리스트 넣고
            _patterns.Add(Pattern1Attack1);
            _patterns.Add(Pattern2Attack1);
            _patterns.Add(Pattern3Attack1);
        }

        void Update()
        {
            if (bossStats.IsDead)
            {
                if (StateMachine.CurrentState != BossDieState)
                {
                    StateMachine.ChangeState(BossDieState);
                }
                return;
            }
            HandleStateTransitions();
            StateMachine.CurrentState.LogicUpdate();
        }
        
        private void HandleStateTransitions()
        {
            if (StateMachine.CurrentState == BossStunState || IsAttacking())
            {
                isPreparingAttack = false;
                idleTimer = 0f;
                return;
            }
            
            if (Target == null)
            {
                if(StateMachine.CurrentState == BossIdleState) return;
                isPreparingAttack = false;
                StateMachine.ChangeState(BossIdleState);
                return;
            }
            
            if (isPreparingAttack == false)
            {
                if (attackRange.IsInAttackRange && isPreparingAttack == false)
                {
                    isPreparingAttack = true;
                    idleTimer = 0f;
                    StateMachine.ChangeState(BossIdleState);
                    audioSource.Play();
                    audioSource.loop = true;
                }
                else
                {
                    StateMachine.ChangeState(BossMoveState);
                }
            }
            else
            {
                idleTimer += Time.deltaTime;

                if (idleTimer < AttackDelay)
                {
                    StateMachine.ChangeState(BossIdleState);
                    RotateTowardsTarget();
                }
                else
                {
                    if (attackRange.IsInAttackRange && isPreparingAttack)
                    {
                        audioSource.Stop();
                        ExecuteRandomPattern();
                        isPreparingAttack = false;
                        audioSource.loop = false;
                        idleTimer = 0f;
                    }
                    else
                    {
                        StateMachine.ChangeState(BossMoveState);
                    }
                }
            }
        }
        
        private bool IsAttacking() => _patterns.Contains(StateMachine.CurrentState as BossState) || 
                                      StateMachine.CurrentState == Pattern1Attack2 || 
                                      StateMachine.CurrentState == Pattern1Attack3 ||
                                      StateMachine.CurrentState == Pattern2Attack2 ||
                                      StateMachine.CurrentState == Pattern3Attack2 ||
                                      StateMachine.CurrentState == BossDieState;
        private void RotateTowardsTarget()
        {
            if (Target == null) return;

            Vector3 direction = (Target.position - transform.position).normalized;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
            }
        }
        
        private void ExecuteRandomPattern()
        {
            if (_patterns.Count == 0) return;
            int randNum = Random.Range(0, _patterns.Count);
            StateMachine.ChangeState(_patterns[randNum]);
        }

        public void IsDie()
        {
            audioSource.Stop();
            audioSource.loop = false;
            Destroy(gameObject, 3);
        } 
        
        void FixedUpdate() => StateMachine.CurrentState.PhysicsUpdate();

        public void isStunned() => StateMachine.ChangeState(BossStunState);
        public void LHit() => lHitBox.EnableDetection();
        public void RHit() => rHitBox.EnableDetection();
        public void PHit() => pHitBox.EnableDetection();
        
        public void PlaySound(string effectName)
        {
            SoundManager.Instance.PlaySFX(effectName, transform.position);
        }
    }
}