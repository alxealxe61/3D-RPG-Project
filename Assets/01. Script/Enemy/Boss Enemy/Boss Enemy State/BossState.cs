using _01._Script.StataPattern;
using UnityEngine;
using UnityEngine.AI;

namespace _01._Script.Enemy.Boss_Enemy.Boss_Enemy_State
{
    public class BossState : State<BossController>
    {
        protected BossController BossEnemy => owner;

        protected readonly NavMeshAgent Agent;
        
        private readonly int animHash;

        protected BossState
            (BossController owner, StateMachine<BossController> stateMachine, string aniName)
            : base(owner, stateMachine, aniName)
        {
            this.stateMachine = stateMachine;
            animHash = Animator.StringToHash(aniName);
            Agent = owner.GetComponent<NavMeshAgent>();
        }

        public override void Enter()
        {
            base.Enter();
            if (animHash == 0) return;
            BossEnemy.ani.SetTrigger(animHash);
        }

        public override void Exit()
        {
            base.Exit();
            if (animHash == 0) return;
            BossEnemy.ani.ResetTrigger(animHash);
        }
        
        protected float GetNormalizedTime()
        {
            AnimatorStateInfo stateInfo = BossEnemy.ani.GetCurrentAnimatorStateInfo(0);
        
            if (!BossEnemy.ani.IsInTransition(0) == false)
            {
                return stateInfo.normalizedTime;
            }
            return 0f;
        }
    }
}