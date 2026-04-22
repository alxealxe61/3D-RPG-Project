using _01._Script.Enemy.Range_Enemy.Range_Enemy_Data;
using _01._Script.Enemy.Range_Enemy.Range_EnemyState;
using _01._Script.Enemy.Range_Enemy.Range_EnemyState.CombatState;
using _01._Script.Enemy.Range_Enemy.Range_EnemyState.CombatState.pattern1;
using UnityEngine;
using UnityEngine.Serialization;

namespace _01._Script.Enemy.Range_Enemy
{
    public class RangeEnemyController : MonoBehaviour
    {
        
        public RangeEnemyStats rangeEnemyStats;
        
        [SerializeField] public Animator ani;
        
        public AttackRange AttackRange => attackRange;
    
        [SerializeField] private DetectionRange detectionRange;
        [SerializeField] private AttackRange attackRange;
    
        public float MoveSpeed => rangeEnemyStats.MoveSpeed;
        
        public Transform Target => detectionRange.detectedTarget;
        
        private RangeEnemyStateMachine StateMachine { get; set; }
        
        
        public Transform firePoint;
        
        private const float ATTACK_DELAY = 4.0f; 
        private bool isPreparingAttack;
        
        #region 상태 머신 모음

        public RangeCombatIdleState  RangeCombatIdleState { get; private set; }
        public RangeCombatMoveState  RangeCombatMoveState { get; private set; }
        public RangeCombatStunState  RangeCombatStunState { get; private set; }
        public Pattern1Attack1 Pattern1Attack1 { get; private set; }
        
        #endregion

        public float idleTimer;
        
        void Awake()
        {
            StateMachine = new RangeEnemyStateMachine();

            RangeCombatIdleState = new RangeCombatIdleState(this, StateMachine, "CombatIdle", false);
            RangeCombatMoveState = new RangeCombatMoveState(this, StateMachine, "CombatMove", false);
            RangeCombatStunState = new RangeCombatStunState(this, StateMachine, "CombatStun", false);
            Pattern1Attack1 = new Pattern1Attack1(this, StateMachine, "Pattern1Attack1", false);
        }
        
        void Start()
        {
            StateMachine.Initialize(RangeCombatIdleState);
        }
        
        void Update()
        {
            HandleStateTransitions();
            StateMachine.CurrentState.LogicUpdate();
        }
        
        private void HandleStateTransitions()
        {
            // 공격 중이거나 스턴 상태일 때는 모든 준비 상태 초기화
            if (StateMachine.CurrentState == RangeCombatStunState || StateMachine.CurrentState == Pattern1Attack1)
            {
                isPreparingAttack = false;
                idleTimer = 0f;
                return;
            }

            if (Target == null)
            {
                isPreparingAttack = false;
                StateMachine.ChangeState(RangeCombatIdleState);
                return;
            }

            // 공격 준비 상태가 아닐 때: 공격 범위 안에 들어오면 준비 시작
            if (isPreparingAttack == false)
            {
                if (attackRange.IsInAttackRange)
                {
                    isPreparingAttack = true;
                    idleTimer = 0f;
                    StateMachine.ChangeState(RangeCombatIdleState);
                }
                else
                {
                    // 범위 밖이면 일반 추격
                    StateMachine.ChangeState(RangeCombatMoveState);
                }
            }
            // 공격 준비(기 모으기) 상태일 때
            else
            {
                idleTimer += Time.deltaTime;
                
                if (idleTimer < ATTACK_DELAY)
                {
                    // 4초가 되기 전까진 제자리에서 대기(Idle)하며 타겟 응시
                    StateMachine.ChangeState(RangeCombatIdleState);
                    RotateTowardsTarget();
                }
                else
                {
                    // 4초가 지난 시점의 판단
                    if (attackRange.IsInAttackRange)
                    {
                        // 사거리 안이면 즉시 공격 수행 및 상태 리셋
                        StateMachine.ChangeState(Pattern1Attack1);
                        isPreparingAttack = false;
                        idleTimer = 0f;
                    }
                    else
                    {
                        // 사거리 밖이면 쫓아가기 (범위 안에 드는 순간 공격 실행됨)
                        StateMachine.ChangeState(RangeCombatMoveState);
                    }
                }
            }
        }
        
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
        
        void FixedUpdate() => StateMachine.CurrentState.PhysicsUpdate();

        public void isStunned() => StateMachine.ChangeState(RangeCombatStunState);
    }
}