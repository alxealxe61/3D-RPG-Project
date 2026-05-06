using _01._Script.StatePattern;
using UnityEngine;

namespace _01._Script.Player.PlayerState.CombatState
{
    public class CombatPullState : PlayerState
    {
        private float _timer;
        
        protected internal CombatPullState
            (PlayerController player, StateMachine<PlayerController> stateMachine, string animName, bool useBool = false) 
            : base(player, stateMachine, animName, useBool) { }

        protected internal override void Enter()
        {
            base.Enter();
            _timer = 0.0f;
        }

        protected internal override void LogicUpdate()
        {
            base.LogicUpdate();
            
            Player.transform.Translate(Vector3.forward * (15.0f * Time.deltaTime * 0.5f));
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