namespace _01._Script.Enemy.EnemyState.Melee_EnemyState.CombatState
{
    public abstract class EnemyAttackPatten1 : MeleeEnemyState
    {
        public EnemyAttackPatten1
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
        
        public override void Exit()
        {
            base.Exit();
        }
    }
}