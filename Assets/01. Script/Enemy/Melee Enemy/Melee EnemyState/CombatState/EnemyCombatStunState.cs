using UnityEngine;

namespace _01._Script.Enemy.EnemyState.Melee_EnemyState.CombatState
{
    public class EnemyCombatStunState : MeleeEnemyState
    {
        private float stunDuration = 0.1f;
        private float timer;
        
        public EnemyCombatStunState
            (MeleeEnemyController owner, MeleeEnemyStateMachine stateMachine, string aniName, bool useBool)
            : base(owner, stateMachine, aniName, useBool) { }

        public override void Enter()
        {
            base.Enter();
            timer = 0.0f;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            timer += Time.deltaTime;
            if (timer >= stunDuration)
            {
                stateMachine.ChangeState(meleeEnemy.CombatIdleState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}