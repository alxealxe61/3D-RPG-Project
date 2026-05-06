using UnityEngine;

namespace _01._Script.Player.PlayerState.CombatState
{
    public class CombatDodgeState : PlayerState
    {
        protected internal CombatDodgeState
            (PlayerController player, PlayerStateMachine stateMachine, string animName) 
            : base(player, stateMachine, animName) { }

        protected internal override void Enter()
        {
            base.Enter();
            Player.hurtBox.SetActive(false);
            
            if (GetNormalizedTime() < 0.7f)
            {
                Player.transform.position -= Player.transform.forward * (4 * Time.deltaTime);
            }
        }

        protected internal override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (GetNormalizedTime() >= 0.9f)
            {
                if (Player.InputVector.sqrMagnitude > 0.01f)
                {
                    StateMachine.ChangeState(Player.CombatMoveState);
                }
                else
                {
                    StateMachine.ChangeState(Player.CombatIdleState);
                }
            }
            
            
        }

        protected internal override void Exit()
        {
            base.Exit();
            Player.dodgetime = 0.0f;
            Player.isDodge = true;
            Player.hurtBox.SetActive(true);
        }
    }
}
