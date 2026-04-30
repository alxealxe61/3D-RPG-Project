using UnityEngine;

namespace _01._Script.Player.PlayerState.CombatState
{
    public class CombatDodgeState : PlayerState
    {
        protected internal CombatDodgeState
            (PlayerController player, PlayerStateMachine stateMachine, string animName) 
            : base(player, stateMachine, animName) { }

        public override void Enter()
        {
            base.Enter();
            Player.hurtBox.SetActive(false);
            
            if (GetNormalizedTime() < 0.7f)
            {
                Player.transform.position -= Player.transform.forward * (4 * Time.deltaTime);
            }
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (GetNormalizedTime() >= 0.9f)
            {
                if (Player.InputVector.sqrMagnitude > 0.01f)
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
            Player.dodgetime = 0.0f;
            Player.isDodge = true;
            Player.hurtBox.SetActive(true);
        }
    }
}
