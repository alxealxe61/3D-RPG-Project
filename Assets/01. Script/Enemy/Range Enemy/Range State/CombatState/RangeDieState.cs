using UnityEngine;

namespace _01._Script.Enemy.Range_Enemy.Range_EnemyState.CombatState
{
    public class RangeDieState : RangeState
    {
        public RangeDieState
            (RangeController owner, RangeStateMachine stateMachine, string aniName, bool useBool) 
            : base(owner, stateMachine, aniName, useBool) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("죽음");
        }
    }
}