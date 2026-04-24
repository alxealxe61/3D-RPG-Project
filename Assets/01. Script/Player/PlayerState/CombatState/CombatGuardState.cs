using UnityEngine;

namespace _01._Script
{
    public class CombatGuardState : PlayerState
    {
        public CombatGuardState
            (PlayerController player, PlayerStateMachine stateMachine, string animName, bool userBool)
            : base(player, stateMachine, animName) { }

        public override void Enter()
        {
            base.Enter();
            player.GuardTimer = 0.0f;
        }
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            player.UpdateGuardTimer();
            
            if (Input.GetMouseButtonUp(1))
            {
                stateMachine.ChangeState(player.CombatIdleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}