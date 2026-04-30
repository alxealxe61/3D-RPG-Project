using _01._Script.StataPattern;
using UnityEngine;

namespace _01._Script.Player.PlayerState.CombatState
{
    public class CombatPullState : PlayerState
    {
        private const float PullSpeed = 15.0f;
        
        private float _timer;
        
        protected internal CombatPullState
            (PlayerController player, StateMachine<PlayerController> stateMachine, string animName, bool useBool = false) 
            : base(player, stateMachine, animName, useBool) { }
        
        public override void Enter()
        {
            base.Enter();
            _timer = 0.0f;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            Player.transform.Translate(Vector3.forward * (PullSpeed * Time.deltaTime * 0.5f));
            _timer += Time.deltaTime;
            if (_timer >= 0.1f)
            {
                if (Player.InputVector.sqrMagnitude > 0.01f)
                {
                    stateMachine.ChangeState(Player.CombatMoveState);
                }
                else
                {
                    stateMachine.ChangeState(Player.CombatIdleState);
                }
            }
        }
    }
}