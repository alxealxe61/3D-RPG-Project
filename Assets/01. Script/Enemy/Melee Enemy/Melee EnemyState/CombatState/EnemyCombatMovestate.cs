using UnityEngine;

namespace _01._Script.Enemy.EnemyState.Melee_EnemyState.CombatState
{
    public class EnemyCombatMovestate : MeleeEnemyState
    {
        private const float TARGET_UPDATE_INTERVAL = 0.2f;
        private float lastUpdateTime;
        
        public EnemyCombatMovestate
            (MeleeEnemyController owner, MeleeEnemyStateMachine stateMachine, string aniName, bool useBool)
            : base(owner, stateMachine, aniName, useBool) { }

        public override void Enter()
        {
            base.Enter();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (meleeEnemy.target == null) return;

            float distance = Vector3.Distance(meleeEnemy.transform.position, meleeEnemy.target.position);
            if (distance <= meleeEnemy.Agent.stoppingDistance)
            {
                Debug.Log(distance);
                // 공격 가능 거리 도달 시 로직
            }

            if (Time.time - lastUpdateTime > TARGET_UPDATE_INTERVAL)
            {
                UpdatePath();
            }
        }

        private void UpdatePath()
        {
            if (meleeEnemy.Agent.isOnNavMesh)
            {
                meleeEnemy.Agent.SetDestination(meleeEnemy.target.position);
                lastUpdateTime = Time.time;
            }
        }
        
        public override void Exit()
        {
            base.Exit();
            if (meleeEnemy.Agent.isOnNavMesh) meleeEnemy.Agent.ResetPath();
        }
    }
}