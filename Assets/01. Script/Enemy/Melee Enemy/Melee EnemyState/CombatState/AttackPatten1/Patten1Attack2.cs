namespace _01._Script.Enemy.EnemyState.Melee_EnemyState.CombatState.AttackPatten1
{
    public class Patten1Attack2 : MeleeEnemyState
    {
        public Patten1Attack2
            (MeleeEnemyController owner, MeleeEnemyStateMachine stateMachine, string aniName, bool useBool)
            : base(owner, stateMachine, aniName, useBool) { }
        
        public override void Enter()
        {
            base.Enter();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (GetNormalizedTime() >= 0.9f)
            {
                stateMachine.ChangeState(meleeEnemy.combatIdleState);
            }
        }
        
        public override void Exit()
        {
            base.Exit();
            
        }
    }
}