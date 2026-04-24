
using _01._Script.StataPattern;
using UnityEngine;

namespace _01._Script
{
    public class CombatPullState : PlayerState
    {
        private const float PullSpeed = 15.0f;
        
        private float stunDuration = 0.1f;
        private float timer;
        
        public CombatPullState
            (PlayerController player, StateMachine<PlayerController> stateMachine, string animName, bool useBool = false) 
            : base(player, stateMachine, animName, useBool) { }
        
        public override void Enter()
        {
            base.Enter();
            timer = 0.0f;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            player.transform.Translate(Vector3.forward * (PullSpeed * Time.deltaTime));
            timer += Time.deltaTime;
            if (timer >= stunDuration)
            {
                if (player.InputVector.sqrMagnitude > 0.01f)
                {
                    stateMachine.ChangeState(player.CombatMoveState);
                }
                else
                {
                    stateMachine.ChangeState(player.CombatIdleState);
                }
            }
        }
    }
}