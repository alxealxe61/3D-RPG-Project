using UnityEngine;

namespace _01._Script.Enemy.Melee_Enemy.Melee_Enemy_State.CombatState.Pattern2
{
    public class Pattern2Attack2 : MeleeState
    {
        private float _timer;
        
        protected internal Pattern2Attack2
            (MeleeController owner, MeleeStateMachine stateMachine, string aniName) 
            : base(owner, stateMachine, aniName) { }

        protected internal override void Enter()
        {
            base.Enter();
            _timer = 0f;
        }

        protected internal override void LogicUpdate()
        {
            _timer += Time.deltaTime;
            base.LogicUpdate();
            if (_timer >= 1f)
            {
                StateMachine.ChangeState(MeleeEnemy.Pattern2Attack3);
            }
        }

        protected internal override void Exit()
        {
            base.Exit();
            MeleeEnemy.rHitBox.DisableDetection();
        }
    }
}