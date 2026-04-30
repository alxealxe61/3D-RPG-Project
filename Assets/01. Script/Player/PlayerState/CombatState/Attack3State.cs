using UnityEngine;

namespace _01._Script.Player.PlayerState.CombatState
{
    public class Attack3State : CombatAttackState
    {
        protected internal Attack3State
            (PlayerController player, PlayerStateMachine stateMachine, string animName)
            : base(player, stateMachine, animName) { }
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (GetNormalizedTime() >= 0.9f)
            {
                stateMachine.ChangeState(Player.CombatIdleState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            
            if (Input.GetMouseButtonDown(1) && GetNormalizedTime() >= 0.6f)
            {
                stateMachine.ChangeState(Player.CombatGuardState);
            }
        }
    }
}