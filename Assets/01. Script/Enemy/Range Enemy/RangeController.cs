using _01._Script.Data;
using _01._Script.Enemy.Range_Enemy.Bullet;
using _01._Script.Enemy.Range_Enemy.Range_EnemyState;
using _01._Script.Enemy.Range_Enemy.Range_EnemyState.CombatState;
using _01._Script.Enemy.Range_Enemy.Range_EnemyState.CombatState.pattern1;
using UnityEngine;

namespace _01._Script.Enemy.Range_Enemy
{
    public class RangeController : MonoBehaviour
    {
        public RangeStats rangeStats;
        
        [SerializeField] public Animator ani;
        [SerializeField] private DetectionRange detectionRange;
        [SerializeField] private AttackRange attackRange;
        public BulletPool bulletPool;
        
        public float MoveSpeed => rangeStats.MoveSpeed;
        
        public Transform Target => detectionRange.detectedTarget;
        
        private RangeStateMachine StateMachine { get; set; }
        
        public Transform firePoint;
        
        private const float AttackDelay = 2.0f; 
        private bool isPreparingAttack;
        
        public float idleTimer;
        #region 상태 머신 모음

        public RangeIdleState  RangeIdleState { get; private set; }
        public RangeMoveState  RangeMoveState { get; private set; }
        public RangeStunState  RangeStunState { get; private set; }
        public Pattern1Attack1 Pattern1Attack1 { get; private set; }
        public Pattern1Attack2 Pattern1Attack2 { get; private set; }
        public RangeDieState RangeDieState { get; private set; }
        
        #endregion

        
        
        void Awake()
        {
            StateMachine = new RangeStateMachine();

            RangeIdleState = new RangeIdleState(this, StateMachine, "CombatIdle", false);
            RangeMoveState = new RangeMoveState(this, StateMachine, "CombatMove", false);
            RangeStunState = new RangeStunState(this, StateMachine, "CombatStun", false);
            RangeDieState = new RangeDieState(this, StateMachine,"CombatDie",false);
            Pattern1Attack1 = new Pattern1Attack1(this, StateMachine, "Pattern1Attack1", false);
            Pattern1Attack2 = new Pattern1Attack2(this, StateMachine, "Pattern1Attack2", false);
        }
        
        void Start()
        {
            StateMachine.Initialize(RangeIdleState);
        }
        
        void Update()
        {
            if (rangeStats.IsDead)
            {
                if (StateMachine.CurrentState != RangeDieState)
                {
                    StateMachine.ChangeState(RangeDieState);
                }
                return;
            }

            HandleStateTransitions();
            StateMachine.CurrentState.LogicUpdate();
        }
        
        private void HandleStateTransitions()
        {
            // 죽었거나 공격 중, 스턴 상태일 때는 모든 준비 상태 초기화 및 전이 중단
            if (StateMachine.CurrentState == RangeDieState 
                || StateMachine.CurrentState == RangeStunState 
                || StateMachine.CurrentState == Pattern1Attack1
                || StateMachine.CurrentState == Pattern1Attack2)
            {
                isPreparingAttack = false;
                idleTimer = 0f;
                return;
            }

            if (Target == null)
            {
                isPreparingAttack = false;
                StateMachine.ChangeState(RangeIdleState);
                return;
            }

            // 공격 준비 상태가 아닐 때: 공격 범위 안에 들어오면 준비 시작
            if (isPreparingAttack == false)
            {
                if (attackRange.IsInAttackRange)
                {
                    isPreparingAttack = true;
                    idleTimer = 0f;
                    StateMachine.ChangeState(RangeIdleState);
                }
                else
                {
                    // 범위 밖이면 일반 추격
                    StateMachine.ChangeState(RangeMoveState);
                }
            }
            // 공격 준비(기 모으기) 상태일 때
            else
            {
                idleTimer += Time.deltaTime;
                
                if (idleTimer < AttackDelay)
                {
                    // 4초가 되기 전까진 제자리에서 대기(Idle)하며 타겟 응시
                    StateMachine.ChangeState(RangeIdleState);
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
                        StateMachine.ChangeState(RangeMoveState);
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
        
        public void IsDie() => Destroy(gameObject, 1);
        
        void FixedUpdate() => StateMachine.CurrentState.PhysicsUpdate();

        public void isStunned() => StateMachine.ChangeState(RangeStunState);
        
    }
}