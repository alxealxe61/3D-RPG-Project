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
            // 현재 애니메이터의 첫 번째 레이어 상태 정보를 가져옵니다.
            AnimatorStateInfo stateInfo = BossEnemy.ani.GetCurrentAnimatorStateInfo(0);
        
            // 1. 현재 애니메이터가 '전환(Transition)' 중이라면 0을 반환하여 대기하게 합니다.
            // (이전 상태의 높은 NormalizedTime이 새 상태로 흘러 들어오는 것을 방지)
            if (BossEnemy.ani.IsInTransition(0))
            {
                return 0f;
            }

            // 2. 현재 재생 중인 애니메이션 노드의 이름 해시가 이 상태의 해시와 일치하는지 엄격히 확인합니다.
            if (stateInfo.shortNameHash == animHash)
            {
                return stateInfo.normalizedTime;
            }
            
            // 그 외의 경우(아직 목표 애니메이션에 도달하지 않음) 0을 반환합니다.
            return 0f;
        }
    }
}