using _01._Script.StataPattern;
using UnityEngine;

namespace _01._Script
{
    public class CombatStunState : PlayerState
    {
        private float stunDuration = 0.1f;
        private float timer;
        
        public CombatStunState
            (PlayerController player, StateMachine<PlayerController> stateMachine, string animName, bool useBool) 
            : base(player, stateMachine, animName) { }

        public override void Enter()
        {
            base.Enter();
            timer = 0.0f;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            timer += Time.deltaTime;
            if (timer >= stunDuration)
            {
                if (player.InputVector.sqrMagnitude > 0.01f)
                {
                    stateMachine.ChangeState(player.combatMoveState);
                }
                else
                {
                    stateMachine.ChangeState(player.combatIdleState);
                }
            }
        }
    }
}