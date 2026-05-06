using _01._Script.StatePattern;
using UnityEngine;

namespace _01._Script.Player.PlayerState.CombatState
{
    public class CombatStunState : PlayerState
    {
        private float _timer;
        
        protected internal CombatStunState
            (PlayerController player, StateMachine<PlayerController> stateMachine, string animName) 
            : base(player, stateMachine, animName) { }

        protected internal override void Enter()
        {
            base.Enter();
            _timer = 0.0f;
        }

        protected internal override void LogicUpdate()
        {
            base.LogicUpdate();
            _timer += Time.deltaTime;
            if (_timer >= 0.1f)
            {
                if (Player.InputVector.sqrMagnitude > 0.01f)
                {
                    StateMachine.ChangeState(Player.CombatMoveState);
                }
                else
                {
                    StateMachine.ChangeState(Player.CombatIdleState);
                }
            }
        }
    }
}