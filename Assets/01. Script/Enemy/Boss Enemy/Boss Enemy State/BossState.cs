using _01._Script.StatePattern;
using UnityEngine;
using UnityEngine.AI;

namespace _01._Script.Enemy.Boss_Enemy.Boss_Enemy_State
{
    public class BossState : State<BossController>
    {
        protected BossController BossEnemy => Owner;

        protected readonly NavMeshAgent Agent;
        
        private readonly int _animHash;

        protected BossState
            (BossController owner, StateMachine<BossController> stateMachine, string aniName)
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
            BossEnemy.ani.SetTrigger(_animHash);
        }

        protected internal override void Exit()
        {
            base.Exit();
            if (_animHash == 0) return;
            BossEnemy.ani.ResetTrigger(_animHash);
        }
        
        protected float GetNormalizedTime()
        {
            var stateInfo = BossEnemy.ani.GetCurrentAnimatorStateInfo(0);
            
            if (!BossEnemy.ani.IsInTransition(0) && stateInfo.shortNameHash == _animHash)
            {
                return stateInfo.normalizedTime;
            }
            return 0f;
        }
    }
    
    public class BossStateMachine : StateMachine<BossController> { }
}