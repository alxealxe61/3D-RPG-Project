using UnityEngine;

namespace _01._Script.Enemy.Melee_Enemy.Melee_Enemy_State.CombatState.Pattern1
{
    public class Pattern1Attack1 : MeleeState
    {
        private float _timer;
        
        protected internal Pattern1Attack1
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
                StateMachine.ChangeState(MeleeEnemy.Pattern1Attack2);
            }
        }

        protected internal override void Exit()
        {
            base.Exit();
            MeleeEnemy.rHitBox.DisableDetection();
        }
    }
}