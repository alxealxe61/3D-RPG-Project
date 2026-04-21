using UnityEngine;

namespace _01._Script
{
    public class CombatSkillState : PlayerState
    {
        
        public CombatSkillState
            (PlayerController player, PlayerStateMachine stateMachine, string animName, bool userBool) 
            : base(player, stateMachine, animName) { }
        
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

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void Exit()
        {
            base.Exit();
            player.skillHitBox.DisableDetection();
        }

    }
}