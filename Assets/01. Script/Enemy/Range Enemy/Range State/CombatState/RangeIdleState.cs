using UnityEngine;

namespace _01._Script.Enemy.Range_Enemy.Range_State.CombatState
{
    public class RangeIdleState : RangeState
    {
        protected internal RangeIdleState
            (RangeController owner, RangeStateMachine stateMachine, string aniName) 
            : base(owner, stateMachine, aniName) { }

        protected internal override void Enter()
        {
            base.Enter();
            if (Agent != null && Agent.isOnNavMesh)
            {
                Agent.isStopped = true;
                Agent.velocity = Vector3.zero;
            }
        }
    }
}