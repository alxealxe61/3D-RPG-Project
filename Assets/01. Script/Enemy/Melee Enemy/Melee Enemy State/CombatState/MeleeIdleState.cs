using UnityEngine;

namespace _01._Script.Enemy.Melee_Enemy.Melee_Enemy_State.CombatState
{
    public class MeleeIdleState : MeleeState
    {
        protected internal MeleeIdleState
            (MeleeController owner, MeleeStateMachine stateMachine, string aniName)
            : base(owner, stateMachine, aniName) { }

        protected internal override void Enter()
        {
            base.Enter();
            if (Agent == null || !Agent.isOnNavMesh) return;
            Agent.isStopped = true;
            Agent.velocity = Vector3.zero;
        }
    }
}