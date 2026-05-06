using UnityEngine;

namespace _01._Script.Player.PlayerState.CombatState
{
    public class Attack3State : CombatAttackState
    {
        protected internal Attack3State
            (PlayerController player, PlayerStateMachine stateMachine, string animName)
            : base(player, stateMachine, animName) { }

        protected internal override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (GetNormalizedTime() >= 0.9f)
            {
                StateMachine.ChangeState(Player.CombatIdleState);
            }
        }

        protected internal override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            
            if (Input.GetMouseButtonDown(1) && GetNormalizedTime() >= 0.6f)
            {
                StateMachine.ChangeState(Player.CombatGuardState);
            }
        }
    }
}