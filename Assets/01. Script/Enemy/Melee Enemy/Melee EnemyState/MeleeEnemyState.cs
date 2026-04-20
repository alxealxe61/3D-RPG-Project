using System.Buffers;
using _01._Script.StataPattern;
using UnityEngine;

namespace _01._Script.Enemy.EnemyState.Melee_EnemyState
{
    public abstract class MeleeEnemyState : State<MeleeEnemyController>
    {
        protected MeleeEnemyController meleeEnemy => owner;
        //protected new MeleeEnemyStateMachine stateMachine;

        private readonly int animHash;
        private readonly bool useBool;
        protected MeleeEnemyState
            (MeleeEnemyController owner, MeleeEnemyStateMachine stateMachine, string aniName, bool useBool)
            : base(owner, stateMachine, aniName)
        {
            this.useBool = useBool;
            this.stateMachine = stateMachine;
            animHash = Animator.StringToHash(aniName);
        }
        
        public override void Enter()
        {
            if (animHash == 0) return;
            if (useBool)
            {
                meleeEnemy.ani.SetBool(animHash, true);
            }
            else
            {
                meleeEnemy.ani.SetTrigger(animHash);
            }
        }

        public override void Exit()
        {
            if(animHash == 0) return;

            if (useBool)
            {
                meleeEnemy.ani.SetBool(animHash, false);
            }
            else
            {
                meleeEnemy.ani.ResetTrigger(animHash); 
            }
        }
        
        protected float GetNormalizedTime()
        {
            AnimatorStateInfo stateInfo = meleeEnemy.ani.GetCurrentAnimatorStateInfo(0);
        
            if (meleeEnemy.ani.IsInTransition(0) == false)
            {
                return stateInfo.normalizedTime;
            }
            return 0;
        }
    }
}