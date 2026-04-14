using UnityEngine;

namespace _01._Script
{
    public class CombatIdleState : PlayerState
    {
        public CombatIdleState
            (PlayerController player, PlayerStateMachine stateMachine, string animName)
            : base(player, stateMachine, animName) { }

        public override void Enter()
        {
            base.Enter();
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (player.InputVector.sqrMagnitude > 0)
            {
                stateMachine.ChangeState(player.combatMoveState);
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                stateMachine.ChangeState(player.attack1State);
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                stateMachine.BoolChangeState(player.combatGuardState);
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                stateMachine.ChangeState(player.combatDodgeState);
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                stateMachine.ChangeState(player.combatSkillState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}