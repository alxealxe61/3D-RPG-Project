using UnityEngine;

namespace _01._Script.Enemy.Range_Enemy.Range_EnemyState.CombatState
{
    public class RangeCombatIdleState : RangeEnemyState
    {
        public RangeCombatIdleState
            (RangeEnemyController owner, RangeEnemyStateMachine stateMachine, string aniName, bool useBool) 
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