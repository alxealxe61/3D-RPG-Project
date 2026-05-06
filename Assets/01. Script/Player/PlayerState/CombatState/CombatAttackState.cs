using UnityEngine;

namespace _01._Script.Player.PlayerState.CombatState
{
    public abstract class CombatAttackState : PlayerState
    {
        protected bool ComboPossible;
        
        protected internal CombatAttackState
            (PlayerController player, PlayerStateMachine stateMachine, string animName)
            : base(player, stateMachine, animName) { }

        protected internal override void Enter()
        {
            base.Enter();
            ComboPossible = false;
        }

        protected internal override void LogicUpdate()
        {
            base.LogicUpdate();
            if (Input.GetKeyDown(KeyCode.LeftShift) && !Player.isDodge)
            {
                StateMachine.ChangeState(Player.CombatDodgeState);
            }
        }

        protected internal override void Exit()
        {
            base.Exit();
            Player.hitBox.DisableDetection();
            ComboPossible = false;
        }
    }
}