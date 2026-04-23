using _01._Script.Enemy.EnemyState.Melee_EnemyState;
using _01._Script.StataPattern;
using UnityEngine;
using UnityEngine.AI;

namespace _01._Script.Enemy.Melee_Enemy.Melee_EnemyState
{
    public abstract class MeleeState : State<MeleeController>
    {
        protected MeleeController MeleeEnemy => owner;

        protected readonly NavMeshAgent Agent;
        
        private readonly int animHash;
        private readonly bool useBool;
        protected MeleeState
            (MeleeController owner, MeleeStateMachine stateMachine, string aniName, bool useBool)
            : base(owner, stateMachine, aniName)
        {
            this.useBool = useBool;
            this.stateMachine = stateMachine;
            animHash = Animator.StringToHash(aniName);
            Agent = owner.GetComponent<NavMeshAgent>();
        }
        
        public override void Enter()
        {
            if (animHash == 0) return;
            if (useBool)
            {
                MeleeEnemy.ani.SetBool(animHash, true);
            }
            else
            {
                MeleeEnemy.ani.SetTrigger(animHash);
            }
        }

        public override void Exit()
        {
            if(animHash == 0) return;

            if (useBool)
            {
                MeleeEnemy.ani.SetBool(animHash, false);
            }
            else
            {
                MeleeEnemy.ani.ResetTrigger(animHash); 
            }
        }
        
        protected float GetNormalizedTime()
        {
            AnimatorStateInfo stateInfo = MeleeEnemy.ani.GetCurrentAnimatorStateInfo(0);
        
            if (!MeleeEnemy.ani.IsInTransition(0) && stateInfo.shortNameHash == animHash)
            {
                return stateInfo.normalizedTime;
            }
            return 0f;
        }
    }
}