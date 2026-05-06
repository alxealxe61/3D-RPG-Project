using UnityEngine;

namespace _01._Script.Enemy.Range_Enemy.Range_State.CombatState
{
    public class RangeStunState : RangeState
    {
        private float _timer;
        
        protected internal RangeStunState
            (RangeController owner, RangeStateMachine stateMachine, string aniName) 
            : base(owner, stateMachine, aniName) { }

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
                StateMachine.ChangeState(RangeEnemy.RangeIdleState);
            }
        }
    }
}