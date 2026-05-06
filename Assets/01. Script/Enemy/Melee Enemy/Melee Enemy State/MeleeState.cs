using _01._Script.StatePattern;
using UnityEngine;
using UnityEngine.AI;

namespace _01._Script.Enemy.Melee_Enemy.Melee_Enemy_State
{
    public abstract class MeleeState : State<MeleeController>
    {
        protected MeleeController MeleeEnemy => Owner;

        protected readonly NavMeshAgent Agent;
        
        private readonly int _animHash;
        
        protected MeleeState
            (MeleeController owner, MeleeStateMachine stateMachine, string aniName)
            : base(owner, stateMachine, aniName)
        {
            StateMachine = stateMachine;
            _animHash = Animator.StringToHash(aniName);
            Agent = owner.GetComponent<NavMeshAgent>();
        }

        protected internal override void Enter()
        {
            if (_animHash == 0) return;
            MeleeEnemy.ani.SetTrigger(_animHash);
            
        }

        protected internal override void Exit()
        {
            if(_animHash == 0) return;
            MeleeEnemy.ani.ResetTrigger(_animHash); 
        }
        
        protected float GetNormalizedTime()
        {
            var stateInfo = MeleeEnemy.ani.GetCurrentAnimatorStateInfo(0);
        
            if (!MeleeEnemy.ani.IsInTransition(0) && stateInfo.shortNameHash == _animHash)
            {
                return stateInfo.normalizedTime;
            }
            return 0f;
        }
    }
    
    public class MeleeStateMachine : StateMachine<MeleeController> { }
}