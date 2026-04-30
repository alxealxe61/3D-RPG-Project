using UnityEngine;

namespace _01._Script.Player.PlayerState.CombatState
{
    public class Attack2State : CombatAttackState
    {
        protected internal Attack2State
            (PlayerController player, PlayerStateMachine stateMachine, string animName)
            : base(player, stateMachine, animName) { }
        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (Input.GetMouseButtonDown(0) && GetNormalizedTime() >= 0.4f && ComboPossible == false)
            {
                ComboPossible = true;
            }
            
            if (GetNormalizedTime() >= 0.9f)
            {
                if (ComboPossible)
                    stateMachine.ChangeState(Player.Attack3State); 
                else
                    stateMachine.ChangeState(Player.CombatIdleState); 
            }
        }
    }
}