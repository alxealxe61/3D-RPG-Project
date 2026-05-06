using UnityEngine;

namespace _01._Script.Enemy.Melee_Enemy.Melee_Enemy_State.CombatState
{
    public class MeleeStunState : MeleeState
    {
        private float _timer;
        
        protected internal MeleeStunState
            (MeleeController owner, MeleeStateMachine stateMachine, string aniName)
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
                StateMachine.ChangeState(MeleeEnemy.MeleeIdleState);
            }
        }
    }
}