using _01._Script.StataPattern;
using UnityEngine;

namespace _01._Script.Enemy.Boss_Enemy.Boss_Enemy_State.CombatState
{
    public class BossMoveState : BossState
    {
        public BossMoveState
            (BossController owner, StateMachine<BossController> stateMachine, string aniName) 
            : base(owner, stateMachine, aniName) { }
        
        public override void Enter()
        {
            base.Enter();
            Debug.Log("Move");
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