namespace _01._Script.Enemy.Melee_Enemy.Melee_Enemy_State.CombatState.Pattern2
{
    public class Pattern2Attack3 : MeleeState
    {
        protected internal Pattern2Attack3
            (MeleeController owner, MeleeStateMachine stateMachine, string aniName) 
            : base(owner, stateMachine, aniName) { }

        protected internal override void LogicUpdate()
        {
            base.LogicUpdate();
            if (GetNormalizedTime() >= 0.9f)
            {
                StateMachine.ChangeState(MeleeEnemy.MeleeIdleState);
            }
        }

        protected internal override void Exit()
        {
            base.Exit();
            MeleeEnemy.lHitBox.DisableDetection();
        }
    }
}