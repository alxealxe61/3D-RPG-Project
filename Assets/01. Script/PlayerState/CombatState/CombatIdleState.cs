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
            Debug.Log("Idle");
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
                player.ani.SetTrigger("CombatAttack");
                stateMachine.ChangeState(player.combatAttackState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}