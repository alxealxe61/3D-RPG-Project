namespace _01._Script.Player.PlayerState
{
    public class ExitCombatState : PlayerState
    {
        protected internal ExitCombatState
            (PlayerController player, PlayerStateMachine stateMachine, string animName)
            : base(player, stateMachine, animName) { }

        public override void Enter()
        {
            base.Enter();
            Player.isWeaponInHand = false;
        }
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (GetNormalizedTime() >= 0.9f)
            {
                if (Player.InputVector.sqrMagnitude > 0.1f)
                {
                    stateMachine.ChangeState(Player.PeaceMoveState);
                }
                else
                {
                    stateMachine.ChangeState(Player.PeaceIdleState);
                }
            }
        }
    }
}