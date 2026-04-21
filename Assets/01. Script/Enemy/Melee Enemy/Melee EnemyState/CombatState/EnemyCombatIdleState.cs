using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _01._Script.Enemy.EnemyState.Melee_EnemyState.CombatState
{
    public class EnemyCombatIdleState : MeleeEnemyState
    {
        
        
        private float idleTimer;
        private const float IDLE_TIME = 3.0f;
        
        public EnemyCombatIdleState
            (MeleeEnemyController owner, MeleeEnemyStateMachine stateMachine, string aniName, bool useBool)
            : base(owner, stateMachine, aniName, useBool)
        { }
        
        public override void Enter()
        {
            base.Enter();
            idleTimer = 0f;
            if (agent != null && agent.isOnNavMesh)
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
            }
            
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (owner.Target != null)
            {
                // 공격 범위 밖으로 나가면 다시 추격(Move) 상태로 전환
                if (owner.attackRange.IsInAttackRange == false)
                {
                    float distance = Vector3.Distance(owner.transform.position, owner.Target.position);

                    if (distance > 5.0f)
                    {
                        stateMachine.ChangeState(meleeEnemy.CombatMovestate);
                        return;
                    }

                    if (agent != null && agent.isOnNavMesh)
                    {
                        agent.isStopped = false;
                        agent.speed = owner.moveSpeed * 0.5f;
                        
                        agent.SetDestination(owner.Target.position);
                    }
                }
                else
                {
                    if (agent != null && agent.isOnNavMesh)
                    {
                        agent.isStopped = true;
                    }
                }
                
                // 타겟을 향해 회전
                Vector3 direction = (owner.Target.position - owner.transform.position).normalized;
                direction.y = 0; // Y축 회전 방지
                
                if (direction != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, lookRotation, Time.deltaTime * 30f);
                }
            }

            if (owner.Target != null)
            {
                // 일정 시간 대기 후 공격 수행
                idleTimer += Time.deltaTime;
                if (idleTimer >= IDLE_TIME)
                {
                    ExecuteRandomPattern();
                }
            }
        }

        private void ExecuteRandomPattern()
        {
            if (meleeEnemy.patterns.Count == 0) return;
            
            int randNum = Random.Range(0, meleeEnemy.patterns.Count);
            MeleeEnemyState nextPattern = meleeEnemy.patterns[randNum];
            
            stateMachine.ChangeState(nextPattern);
        }
    }
}