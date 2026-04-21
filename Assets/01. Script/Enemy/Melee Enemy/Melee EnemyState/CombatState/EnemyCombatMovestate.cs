using UnityEngine;

namespace _01._Script.Enemy.EnemyState.Melee_EnemyState.CombatState
{
    public class EnemyCombatMovestate : MeleeEnemyState
    {
        public EnemyCombatMovestate
            (MeleeEnemyController owner, MeleeEnemyStateMachine stateMachine, string aniName, bool useBool)
            : base(owner, stateMachine, aniName, useBool) { }

        public override void Enter()
        {
            base.Enter();
            if (agent != null && agent.isOnNavMesh)
            {
                agent.isStopped = false;
                agent.speed = owner.moveSpeed;
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            // 타겟이 사라지면 Idle로 복귀
            if (owner.Target == null)
            {
                stateMachine.ChangeState(meleeEnemy.CombatIdleState);
                return;
            }

            // 공격 범위 안에 들어오면 공격 대기(Idle) 상태로 전환
            if (owner.attackRange.IsInAttackRange)
            {
                stateMachine.ChangeState(meleeEnemy.CombatIdleState);
                return;
            }

            // 타겟을 향해 실시간으로 경로 갱신
            if (agent != null && agent.isOnNavMesh)
            {
                agent.isStopped = false;
                agent.speed = owner.moveSpeed;
                agent.SetDestination(owner.Target.position);
            }
        }

        
        public override void Exit()
        {
            base.Exit();
            if (agent != null && agent.isOnNavMesh)
            {
                agent.isStopped = true;
            }
        }
    }
}