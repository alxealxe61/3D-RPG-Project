using _01._Script.StataPattern;
using UnityEngine;

namespace _01._Script.Enemy.Boss_Enemy.Boss_Enemy_State.CombatState
{
    public class BossIdleState : BossState
    {
        public BossIdleState
            (BossController owner, StateMachine<BossController> stateMachine, string aniName) 
            : base(owner, stateMachine, aniName) { }
        
        public override void Enter()
        {
            base.Enter();
            Debug.Log("Idle");
            if (Agent != null && Agent.isOnNavMesh)
            {
                Agent.isStopped = true;
                Agent.velocity = Vector3.zero;
            }
        }
    }
}