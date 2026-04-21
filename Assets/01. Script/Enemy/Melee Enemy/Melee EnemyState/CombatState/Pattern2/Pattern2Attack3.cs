namespace _01._Script.Enemy.EnemyState.Melee_EnemyState.CombatState.Pattern2
{
    public class Pattern2Attack3 : MeleeEnemyState
    {
        public Pattern2Attack3
            (MeleeEnemyController owner, MeleeEnemyStateMachine stateMachine, string aniName, bool useBool) 
            : base(owner, stateMachine, aniName, useBool) { }
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (GetNormalizedTime() >= 0.9f)
            {
                //meleeEnemy.isAttacking = false;
                stateMachine.ChangeState(meleeEnemy.CombatIdleState);
            }
        }
        
        public override void Exit()
        {
            base.Exit();
            meleeEnemy.lHitBox.DisableDetection();
        }
    }
}