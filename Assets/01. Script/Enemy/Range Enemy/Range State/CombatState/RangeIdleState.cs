using UnityEngine;

namespace _01._Script.Enemy.Range_Enemy.Range_EnemyState.CombatState
{
    public class RangeIdleState : RangeState
    {
        public RangeIdleState
            (RangeController owner, RangeStateMachine stateMachine, string aniName, bool useBool) 
            : base(owner, stateMachine, aniName, useBool) { }
        
        public override void Enter()
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