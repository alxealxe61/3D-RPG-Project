using System.Collections.Generic;
using _01._Script.Data;
using _01._Script.Enemy.Melee_Enemy.Melee_Enemy_State;
using _01._Script.Enemy.Melee_Enemy.Melee_Enemy_State.CombatState;
using _01._Script.Enemy.Melee_Enemy.Melee_Enemy_State.CombatState.Pattern1;
using _01._Script.Enemy.Melee_Enemy.Melee_Enemy_State.CombatState.Pattern2;
using UnityEngine;

namespace _01._Script.Enemy.Melee_Enemy
{
    public class MeleeController : MonoBehaviour
    {
        [SerializeField] public MeleeStats meleeStats;
        [SerializeField] public Animator ani;
        [SerializeField] private DetectionRange detectionRange;
        [SerializeField] private AttackRange attackRange;
    
        public float MoveSpeed => meleeStats.MoveSpeed;
        
        [Header("HitBox")]
        public LHitBox lHitBox;
        public RHitBox rHitBox;

        private readonly List<MeleeState> _patterns = new List<MeleeState>();
        public Transform Target => detectionRange.detectedTarget;
    
        #region 상태 머신 모음
        private MeleeStateMachine StateMachine { get; set; }
        
        public MeleeIdleState MeleeIdleState { get; private set; }
        private MeleeMoveState MeleeMoveState { get; set; }
        private MeleeStunState MeleeStunState { get; set; }
        
        private Pattern1Attack1 Pattern1Attack1 { get; set; }
        public Pattern1Attack2 Pattern1Attack2 { get; private set; }
        private Pattern2Attack1 Pattern2Attack1 { get; set; }
        public Pattern2Attack2 Pattern2Attack2 { get; private set; }
        public Pattern2Attack3 Pattern2Attack3 { get; private set; }
        private MeleeDieState MeleeDieState {  get; set; }
    
        #endregion
        
        private float _idleTimer;
        private bool _isPreparingAttack; // 공격 준비 중인지 여부

        private void Awake()
        {
            StateMachine = new MeleeStateMachine();
        
            MeleeIdleState = new MeleeIdleState(this, StateMachine, "CombatIdle");
            MeleeMoveState = new MeleeMoveState(this, StateMachine, "CombatMove");
            MeleeStunState = new MeleeStunState(this, StateMachine, "CombatStun");
            
            Pattern1Attack1 = new Pattern1Attack1(this, StateMachine, "Pattern1Attack1");
            Pattern1Attack2 = new Pattern1Attack2(this, StateMachine, "Pattern1Attack2");
            Pattern2Attack1 = new Pattern2Attack1(this, StateMachine, "Pattern2Attack1");
            Pattern2Attack2 = new Pattern2Attack2(this, StateMachine, "Pattern2Attack2");
            Pattern2Attack3 = new Pattern2Attack3(this, StateMachine, "Pattern2Attack3");
            
            MeleeDieState = new MeleeDieState(this, StateMachine, "CombatDie");
        }

        private void Start()
        {
            StateMachine.Initialize(MeleeIdleState);
            _patterns.Add(Pattern1Attack1);
            _patterns.Add(Pattern2Attack1);
        }

        private void Update()
        {
            if (meleeStats.IsDead)
            {
                if (StateMachine.CurrentState != MeleeDieState)
                {
                    StateMachine.ChangeState(MeleeDieState);
                }
                return;
            }
            HandleStateTransitions();
            StateMachine.CurrentState.LogicUpdate();
        }

        private void HandleStateTransitions()
        {
            // 공격 중이거나 스턴 상태일 때는 모든 준비 상태 초기화
            if (StateMachine.CurrentState == MeleeStunState || IsAttacking())
            {
                _isPreparingAttack = false;
                _idleTimer = 0f;
                return;
            }

            if (Target == null)
            {
                _isPreparingAttack = false;
                StateMachine.ChangeState(MeleeIdleState);
                return;
            }

            // 공격 준비 상태가 아닐 때: 공격 범위 안에 들어오면 준비 시작
            if (_isPreparingAttack == false)
            {
                if (attackRange.IsInAttackRange)
                {
                    _isPreparingAttack = true;
                    _idleTimer = 0f;
                    StateMachine.ChangeState(MeleeIdleState);
                }
                else
                {
                    // 범위 밖이면 일반 추격
                    StateMachine.ChangeState(MeleeMoveState);
                }
            }
            // 공격 준비(기 모으기) 상태일 때
            else
            {
                _idleTimer += Time.deltaTime;
                
                if (_idleTimer < 4.0f)
                {
                    // 2초가 되기 전까진 제자리에서 대기(Idle)하며 타겟 응시
                    StateMachine.ChangeState(MeleeIdleState);
                    RotateTowardsTarget();
                }
                else
                {
                    // 2초가 지난 시점의 판단
                    if (attackRange.IsInAttackRange)
                    {
                        // 사거리 안이면 즉시 공격 수행 및 상태 리셋
                        ExecuteRandomPattern();
                        _isPreparingAttack = false;
                        _idleTimer = 0f;
                    }
                    else
                    {
                        // 사거리 밖이면 쫓아가기 (범위 안에 드는 순간 공격 실행됨)
                        StateMachine.ChangeState(MeleeMoveState);
                    }
                }
            }
        }
        
        private bool IsAttacking() => _patterns.Contains(StateMachine.CurrentState as MeleeState) || 
                                      StateMachine.CurrentState == Pattern1Attack2 || 
                                      StateMachine.CurrentState == Pattern2Attack2 || 
                                      StateMachine.CurrentState == Pattern2Attack3 ||
                                      StateMachine.CurrentState == MeleeDieState;

        private void RotateTowardsTarget()
        {
            if (Target == null) return;

            var direction = (Target.position - transform.position).normalized;
            direction.y = 0;
            if (direction == Vector3.zero) return;
            var lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }

        private void ExecuteRandomPattern()
        {
            if (_patterns.Count == 0) return;
            var randNum = Random.Range(0, _patterns.Count);
            StateMachine.ChangeState(_patterns[randNum]);
        }

        private void FixedUpdate() => StateMachine.CurrentState.PhysicsUpdate();

        public void IsStunned() => StateMachine.ChangeState(MeleeStunState);

        public void IsDie() => Destroy(gameObject, 1);

        #region 애니메이션 이벤트 함수

        public void LHit() => lHitBox.EnableDetection();
        public void RHit() => rHitBox.EnableDetection();
        
        public void PlaySound(string effectName)
        {
            SoundManager.Instance.PlaySFX(effectName, transform.position);
        }

        #endregion
    }
}