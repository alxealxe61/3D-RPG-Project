using _01._Script.StatePattern;
using UnityEngine;
using UnityEngine.AI;

namespace _01._Script.Enemy.Range_Enemy.Range_State
{
    public class RangeState : State<RangeController>
    {
        protected RangeController RangeEnemy => Owner;

        protected readonly NavMeshAgent Agent;
        
        private readonly int _animHash;
        
        protected RangeState
            (RangeController owner, RangeStateMachine stateMachine, string aniName)
            : base(owner, stateMachine, aniName)
        {
            this.StateMachine = stateMachine;
            _animHash = Animator.StringToHash(aniName);
            Agent = owner.GetComponent<NavMeshAgent>();
        }

        protected internal override void Enter()
        {
            base.Enter();
            if (_animHash == 0) return;
            RangeEnemy.ani.SetTrigger(_animHash);
        }

        protected internal override void Exit()
        {
            base.Exit();
            if(_animHash == 0) return;
            RangeEnemy.ani.ResetTrigger(_animHash); 
        }
        
        protected float GetNormalizedTime()
        {
            var stateInfo = RangeEnemy.ani.GetCurrentAnimatorStateInfo(0);
        
            if (!RangeEnemy.ani.IsInTransition(0) && stateInfo.shortNameHash == _animHash)
            {
                return stateInfo.normalizedTime;
            }
            return 0f;
        }
    }
    
    public class RangeStateMachine : StateMachine<RangeController> { }
}