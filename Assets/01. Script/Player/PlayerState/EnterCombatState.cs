namespace _01._Script.Player.PlayerState
{
    public class EnterCombatState : PlayerState
    {
        protected internal EnterCombatState
            (PlayerController player, PlayerStateMachine stateMachine, string animName) 
            : base(player, stateMachine, animName) { }

        public override void Enter()
        {
            base.Enter();
            Player.isWeaponInHand = true;
            Player.ani.applyRootMotion = true;
        }
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (GetNormalizedTime() >= 0.9f)
            {
                if (Player.InputVector.sqrMagnitude > 0.1f)
                {
                    stateMachine.ChangeState(Player.CombatMoveState);
                }
                else
                {
                    stateMachine.ChangeState(Player.CombatIdleState);
                }
            }
        }
    }
}