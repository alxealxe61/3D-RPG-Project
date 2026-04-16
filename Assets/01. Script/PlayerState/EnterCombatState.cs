namespace _01._Script
{
    public class EnterCombatState : PlayerState
    {
        public EnterCombatState
            (PlayerController player, PlayerStateMachine stateMachine, string animName) 
            : base(player, stateMachine, animName) { }

        public override void Enter()
        {
            base.Enter();
            player.isWeaponInHand = true;
        }
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (GetNormalizedTime() >= 0.9f)
            {
                if (player.InputVector.sqrMagnitude > 0.1f)
                {
                    stateMachine.ChangeState(player.combatMoveState);
                }
                else
                {
                    stateMachine.ChangeState(player.combatIdleState);
                }
            }
        }
    }
}