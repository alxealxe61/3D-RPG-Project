using UnityEngine;

namespace _01._Script.Player.PlayerState.CombatState
{
    public class Attack1State : CombatAttackState
    {
        protected internal Attack1State
            (PlayerController player, PlayerStateMachine stateMachine, string animName) 
            : base(player, stateMachine, animName) { }

        protected internal override void LogicUpdate()
        {
            base.LogicUpdate();

            if (Input.GetMouseButtonDown(0) && GetNormalizedTime() >= 0.4f && ComboPossible == false)
            {
                ComboPossible = true;
            }
            
            if (GetNormalizedTime() >= 0.9f)
            {
                if(ComboPossible)
                    StateMachine.ChangeState(Player.Attack2State);
                else
                    StateMachine.ChangeState(Player.CombatIdleState);
            }
        }
    }
}