using System;
using System.Collections.Generic;
using _01._Script.Enemy.Boss_Enemy.Boss_Enemy_Data;
using _01._Script.Enemy.Boss_Enemy.Boss_Enemy_State;
using _01._Script.Enemy.Boss_Enemy.Boss_Enemy_State.CombatState;
using _01._Script.Enemy.Boss_Enemy.Boss_Enemy_State.CombatState.Pattern1;
using UnityEngine;

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
        
        public readonly List<BossState> Patterns = new List<BossState>();
        
        public Transform firePoint;
        
        [Header("HitBoxes")]
        public LHitBox lHitBox;
        public RHitBox rHitBox;
        
        private const float AttackDelay = 4.0f; 
        public bool isPreparingAttack;

        public float idleTimer;
        #region 상태 머신 모음
        private BossStateMachine StateMachine { get; set; }
        
        public BossIdleState BossIdleState { get; private set; }
        public BossMoveState BossMoveState { get; private set; }
        public BossStunState BossStunState { get; private set; }
        public Pattern1Attack1 Pattern1Attack1 { get; private set; }
        public Pattern1Attack2 Pattern1Attack2 { get; private set; }
        public Pattern1Attack3 Pattern1Attack3 { get; private set; }

        //패턴 1 
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
        }

        private void Start()
        {
            StateMachine.Initialize(BossIdleState);
            // 여기에 패턴들 리스트 넣고
            Patterns.Add(Pattern1Attack1);
        }

        void Update()
        {
            HandleStateTransitions();
            StateMachine.CurrentState.LogicUpdate();
        }
        
        private void HandleStateTransitions()
        {
            // 공격 중이거나 스턴 상태일 때는 모든 준비 상태 초기화
            if (StateMachine.CurrentState == BossStunState || IsAttacking())
            {
                isPreparingAttack = false;
                idleTimer = 0f;
                return;
            }

            if (Target == null)
            {
                isPreparingAttack = false;
                StateMachine.ChangeState(BossIdleState);
                return;
            }

            // 공격 준비 상태가 아닐 때: 공격 범위 안에 들어오면 준비 시작
            if (isPreparingAttack == false)
            {
                if (attackRange.IsInAttackRange)
                {
                    isPreparingAttack = true;
                    idleTimer = 0f;
                    StateMachine.ChangeState(BossIdleState);
                }
                else
                {
                    // 범위 밖이면 일반 추격
                    StateMachine.ChangeState(BossMoveState);
                }
            }
            // 공격 준비(기 모으기) 상태일 때
            else
            {
                idleTimer += Time.deltaTime;
                
                if (idleTimer < AttackDelay)
                {
                    // 4초가 되기 전까진 제자리에서 대기(Idle)하며 타겟 응시
                    StateMachine.ChangeState(BossIdleState);
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
                        StateMachine.ChangeState(BossMoveState);
                    }
                }
            }
        }
        
        private bool IsAttacking() => Patterns.Contains(StateMachine.CurrentState as BossState) || 
                                      StateMachine.CurrentState == Pattern1Attack2 || 
                                      StateMachine.CurrentState == Pattern1Attack2 || 
                                      StateMachine.CurrentState == Pattern1Attack3;
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

        public void LHit() => lHitBox.EnableDetection();
        public void RHit() => rHitBox.EnableDetection();
    }
}