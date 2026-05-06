using UnityEngine;

namespace _01._Script.Player.PlayerState.CombatState
{
    public class CombatGuardState : PlayerState
    {
        protected internal CombatGuardState
            (PlayerController player, PlayerStateMachine stateMachine, string animName, bool userBool)
            : base(player, stateMachine, animName, userBool) { }

        protected internal override void Enter()
        {
            base.Enter();
            Player.GuardTimer = 0.0f;
        }

        protected internal override void LogicUpdate()
        {
            base.LogicUpdate();
            Player.UpdateGuardTimer();
            
            if (Input.GetMouseButtonUp(1))
            {
                StateMachine.ChangeState(Player.CombatIdleState);
            }
        }
    }
}