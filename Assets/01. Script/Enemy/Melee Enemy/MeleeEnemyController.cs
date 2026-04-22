using System.Collections.Generic;
using _01._Script.Enemy_Data;
using _01._Script.Enemy.EnemyState.Melee_EnemyState;
using _01._Script.Enemy.EnemyState.Melee_EnemyState.CombatState;
using _01._Script.Enemy.EnemyState.Melee_EnemyState.CombatState.AttackPatten1;
using _01._Script.Enemy.EnemyState.Melee_EnemyState.CombatState.Pattern2;
using _01._Script.Enemy.Melee_Enemy.Melee_EnemyState;
using UnityEngine;
using UnityEngine.AI;

namespace _01._Script.Enemy.Melee_Enemy
{
    public class MeleeEnemyController : MonoBehaviour
    {
        [Header("몬스터 스텟")]
        public MeleeEnemyStats meleeEnemyStats;

        [SerializeField] public Animator ani;
        public AttackRange AttackRange => attackRange;
    
        [SerializeField] private DetectionRange detectionRange;
        [SerializeField] private AttackRange attackRange;
    
        public float MoveSpeed => meleeEnemyStats.MoveSpeed;
        
        [Header("HitBox")]
        public LHitBox lHitBox;
        public RHitBox rHitBox;

        public readonly List<MeleeEnemyState> Patterns = new List<MeleeEnemyState>();
        public Transform Target => detectionRange.detectedTarget;
    
        #region 상태 머신 모음

        public EnemyCombatIdleState CombatIdleState { get; private set; }
        public EnemyCombatMovestate CombatMovestate { get; private set; }
        public EnemyCombatStunState CombatStunState { get; private set; }
        
        private Pattern1Attack1 Pattern1Attack1 { get; set; }
        public Pattern1Attack2 Pattern1Attack2 { get; private set; }
        private Pattern2Attack1 Pattern2Attack1 { get; set; }
        public Pattern2Attack2 Pattern2Attack2 { get; private set; }
        public Pattern2Attack3 Pattern2Attack3 { get; private set; }
    
        #endregion
    
        private MeleeEnemyStateMachine StateMachine { get; set; }
        
        private float idleTimer;
        private const float ATTACK_DELAY = 4.0f; 
        private bool isPreparingAttack; // 공격 준비 중인지 여부
    
        void Awake()
        {
            StateMachine = new MeleeEnemyStateMachine();
        
            CombatIdleState = new EnemyCombatIdleState(this, StateMachine, "CombatIdle", false);
            CombatMovestate = new EnemyCombatMovestate(this, StateMachine, "CombatMove", false);
            CombatStunState = new EnemyCombatStunState(this, StateMachine, "CombatStun", false);
            
            Pattern1Attack1 = new Pattern1Attack1(this, StateMachine, "Pattern1Attack1", false);
            Pattern1Attack2 = new Pattern1Attack2(this, StateMachine, "Pattern1Attack2", false);
            
            Pattern2Attack1 = new Pattern2Attack1(this, StateMachine, "Pattern2Attack1", false);
            Pattern2Attack2 = new Pattern2Attack2(this, StateMachine, "Pattern2Attack2", false);
            Pattern2Attack3 = new Pattern2Attack3(this, StateMachine, "Pattern2Attack3", false);
        }

        void Start()
        {
            StateMachine.Initialize(CombatIdleState);
            Patterns.Add(Pattern1Attack1);
            Patterns.Add(Pattern2Attack1);
        }

        void Update()
        {
            HandleStateTransitions();
            StateMachine.CurrentState.LogicUpdate();
        }

        private void HandleStateTransitions()
        {
            // 공격 중이거나 스턴 상태일 때는 모든 준비 상태 초기화
            if (StateMachine.CurrentState == CombatStunState || IsAttacking())
            {
                isPreparingAttack = false;
                idleTimer = 0f;
                return;
            }

            if (Target == null)
            {
                isPreparingAttack = false;
                StateMachine.ChangeState(CombatIdleState);
                return;
            }

            // 공격 준비 상태가 아닐 때: 공격 범위 안에 들어오면 준비 시작
            if (isPreparingAttack == false)
            {
                if (attackRange.IsInAttackRange)
                {
                    isPreparingAttack = true;
                    idleTimer = 0f;
                    StateMachine.ChangeState(CombatIdleState);
                }
                else
                {
                    // 범위 밖이면 일반 추격
                    StateMachine.ChangeState(CombatMovestate);
                }
            }
            // 공격 준비(기 모으기) 상태일 때
            else
            {
                idleTimer += Time.deltaTime;
                
                if (idleTimer < ATTACK_DELAY)
                {
                    // 2초가 되기 전까진 제자리에서 대기(Idle)하며 타겟 응시
                    StateMachine.ChangeState(CombatIdleState);
                    RotateTowardsTarget();
                }
                else
                {
                    // 2초가 지난 시점의 판단
                    if (attackRange.IsInAttackRange)
                    {
                        // 사거리 안이면 즉시 공격 수행 및 상태 리셋
                        ExecuteRandomPattern();
                        isPreparingAttack = false;
                        idleTimer = 0f;
                    }
                    else
                    {
                        // 사거리 밖이면 쫓아가기 (범위 안에 드는 순간 공격 실행됨)
                        StateMachine.ChangeState(CombatMovestate);
                    }
                }
            }
        }
        
        private bool IsAttacking() => Patterns.Contains(StateMachine.CurrentState as MeleeEnemyState) || 
                                      StateMachine.CurrentState == Pattern1Attack2 || 
                                      StateMachine.CurrentState == Pattern2Attack2 || 
                                      StateMachine.CurrentState == Pattern2Attack3;

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
            if (Patterns.Count == 0) return;
            int randNum = Random.Range(0, Patterns.Count);
            StateMachine.ChangeState(Patterns[randNum]);
        }

        void FixedUpdate() => StateMachine.CurrentState.PhysicsUpdate();

        public void isStunned() => StateMachine.ChangeState(CombatStunState);

        public void LHit() => lHitBox.EnableDetection();
        public void RHit() => rHitBox.EnableDetection();
    }
}