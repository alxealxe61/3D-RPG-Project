using _01._Script.Enemy.Melee_Enemy;
using _01._Script.Enemy.Melee_Enemy.Melee_EnemyState;
using UnityEngine;

namespace _01._Script.Enemy.EnemyState.Melee_EnemyState.CombatState
{
    public class MeleeMovestate : MeleeState
    {
        public MeleeMovestate
            (MeleeController owner, MeleeStateMachine stateMachine, string aniName, bool useBool)
            : base(owner, stateMachine, aniName, useBool) { }

        public override void Enter()
        {
            base.Enter();
            if (Agent != null && Agent.isOnNavMesh)
            {
                Agent.isStopped = false;
                Agent.speed = owner.MoveSpeed;
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (owner.Target != null && Agent != null && Agent.isOnNavMesh)
            {
                Agent.SetDestination(owner.Target.position);
            }
        }
        
        public override void Exit()
        {
            base.Exit();
            if (Agent != null && Agent.isOnNavMesh)
            {
                Agent.isStopped = true;
            }
        }
    }
}