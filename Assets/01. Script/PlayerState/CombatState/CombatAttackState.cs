
using UnityEngine;

namespace _01._Script
{
    public abstract class CombatAttackState : PlayerState
    {
        //protected float lastInputTime;
        protected bool comboPossible;
        public CombatAttackState(PlayerController player, PlayerStateMachine stateMachine, string animName)
            : base(player, stateMachine, animName) { }

        public override void Enter()
        {
            base.Enter();
            comboPossible = false;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (Input.GetMouseButtonDown(1))
            {
                stateMachine.BoolChangeState(player.combatGuardState);
            }
        }
    }
}