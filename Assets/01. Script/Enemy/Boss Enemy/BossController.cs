using System;
using System.Collections.Generic;
using _01._Script.Enemy.Boss_Enemy.Boss_Enemy_Data;
using _01._Script.Enemy.Boss_Enemy.Boss_Enemy_State;
using _01._Script.Enemy.Boss_Enemy.Boss_Enemy_State.CombatState;
using _01._Script.Enemy.Boss_Enemy.Boss_Enemy_State.CombatState.Pattern1;
using _01._Script.Enemy.Boss_Enemy.Boss_Enemy_State.CombatState.Pattern2;
using _01._Script.Enemy.Boss_Enemy.Boss_Enemy_State.CombatState.Pattern3;
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
        
        public readonly List<BossState> Patterns = new List<BossState>();
        
        public Transform firePoint;
        
        [Header("HitBoxes")]
        public LHitBox lHitBox;
        public RHitBox rHitBox;
        public PullHitBox pHitBox;
        
        public GameObject fireObject;
        
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
        
        public Pattern2Attack1 Pattern2Attack1 { get; private set; }
        public Pattern2Attack2 Pattern2Attack2 { get; private set; }
        public Pattern3Attack1 Pattern3Attack1 { get; private set; }
        public Pattern3Attack2 Pattern3Attack2 { get; private set; }
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

        }

        private void Start()
        {
            StateMachine.Initialize(BossIdleState);
            // 여기에 패턴들 리스트 넣고
            Patterns.Add(Pattern1Attack1);
            Patterns.Add(Pattern2Attack1);
            Patterns.Add(Pattern3Attack1);
        }

        void Update()
        {
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
                        ExecuteRandomPattern();
                        isPreparingAttack = false;
                        idleTimer = 0f;
                    }
                    else
                    {
                        StateMachine.ChangeState(BossMoveState);
                    }
                }
            }
        }
        
        private bool IsAttacking() => Patterns.Contains(StateMachine.CurrentState as BossState) || 
                                      StateMachine.CurrentState == Pattern1Attack2 || 
                                      StateMachine.CurrentState == Pattern1Attack3 ||
                                      StateMachine.CurrentState == Pattern2Attack2 ||
                                      StateMachine.CurrentState == Pattern3Attack2;
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

        public void isStunned() => StateMachine.ChangeState(BossStunState);
        public void LHit() => lHitBox.EnableDetection();
        public void RHit() => rHitBox.EnableDetection();
        public void PHit() => pHitBox.EnableDetection();
    }
}