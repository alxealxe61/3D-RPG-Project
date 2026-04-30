namespace _01._Script.Player.PlayerState.PeaceState
{
    public class PeaceIdleState : PlayerState
    {
        protected internal PeaceIdleState
            (PlayerController player, PlayerStateMachine stateMachine, string animName) 
            : base(player, stateMachine, animName) { }
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();

            // WASD 이동 입력 시 PeaceMoveState로 전환
            if (Player.InputVector.sqrMagnitude > 0)
            {
                stateMachine.ChangeState(Player.PeaceMoveState);
            }
            
            if (Player.lockOnSystem.IsLockedOn)
            {
                stateMachine.ChangeState(Player.EnterCombatState);
            }
        }
    }
}