namespace _01._Script
{
    public class ExitCombatState : PlayerState
    {
        public ExitCombatState
            (PlayerController player, PlayerStateMachine stateMachine, string animName)
            : base(player, stateMachine, animName) { }

        public override void Enter()
        {
            base.Enter();
            player.isWeaponInHand = false;
        }
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (GetNormalizedTime() >= 0.9f)
            {
                if (player.InputVector.sqrMagnitude > 0.1f)
                {
                    stateMachine.ChangeState(player.peaceMoveState);
                }
                else
                {
                    stateMachine.ChangeState(player.peaceIdleState);
                }
            }
        }
    }
}