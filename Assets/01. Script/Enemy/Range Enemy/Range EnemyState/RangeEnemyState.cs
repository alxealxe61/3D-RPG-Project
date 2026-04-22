using _01._Script.StataPattern;
using UnityEngine;
using UnityEngine.AI;

namespace _01._Script.Enemy.Range_Enemy.Range_EnemyState
{
    public class RangeEnemyState : State<RangeEnemyController>
    {
        protected RangeEnemyController RangeEnemy => owner;

        protected readonly NavMeshAgent Agent;

        private readonly bool useBool;
        
        private readonly int animHash;
        protected RangeEnemyState
            (RangeEnemyController owner, RangeEnemyStateMachine stateMachine, string aniName, bool useBool)
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
            
            if (animHash == 0) return;
            if (useBool)
            {
                RangeEnemy.ani.SetBool(animHash, true);
            }
            else
            {
                RangeEnemy.ani.SetTrigger(animHash);
            }
        }

        public override void Exit()
        {
            if(animHash == 0) return;
            
            if (useBool)
            {
                RangeEnemy.ani.SetBool(animHash, false);
            }
            else
            {
                RangeEnemy.ani.ResetTrigger(animHash); 
            }
        }
        
        protected float GetNormalizedTime()
        {
            AnimatorStateInfo stateInfo = RangeEnemy.ani.GetCurrentAnimatorStateInfo(0);
        
            if (!RangeEnemy.ani.IsInTransition(0) && stateInfo.shortNameHash == animHash)
            {
                return stateInfo.normalizedTime;
            }
            return 0f;
        }
    }
}