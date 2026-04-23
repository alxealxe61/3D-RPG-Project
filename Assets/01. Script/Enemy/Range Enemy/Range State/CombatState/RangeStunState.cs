using UnityEngine;

namespace _01._Script.Enemy.Range_Enemy.Range_EnemyState.CombatState
{
    public class RangeStunState : RangeState
    {
        private float stunDuration = 0.1f;
        private float timer;
        
        public RangeStunState
            (RangeController owner, RangeStateMachine stateMachine, string aniName,  bool useBool) 
            : base(owner, stateMachine, aniName, useBool) { }
        
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
                stateMachine.ChangeState(RangeEnemy.RangeIdleState);
            }
        }
    }
}