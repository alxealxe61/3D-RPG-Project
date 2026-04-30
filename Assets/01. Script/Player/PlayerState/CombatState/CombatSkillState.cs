namespace _01._Script.Player.PlayerState.CombatState
{
    public class CombatSkillState : PlayerState
    {
        protected internal CombatSkillState
            (PlayerController player, PlayerStateMachine stateMachine, string animName) 
            : base(player, stateMachine, animName) { }
        
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
        
        public override void Exit()
        {
            base.Exit();
            Player.skillHitBox.DisableDetection();
        }
    }
}