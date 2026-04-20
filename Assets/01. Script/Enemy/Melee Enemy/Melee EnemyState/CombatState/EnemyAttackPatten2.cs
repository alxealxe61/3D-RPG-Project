namespace _01._Script.Enemy.EnemyState.Melee_EnemyState.CombatState
{
    public class EnemyAttackPatten2 : MeleeEnemyState
    {
        public EnemyAttackPatten2
            (MeleeEnemyController owner, MeleeEnemyStateMachine stateMachine, string aniName, bool useBool) 
            : base(owner, stateMachine, aniName, useBool) { }

        public override void Enter()
        {
            base.Enter();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
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