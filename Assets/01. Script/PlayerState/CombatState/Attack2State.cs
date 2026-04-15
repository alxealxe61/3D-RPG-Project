using UnityEngine;

namespace _01._Script
{
    public class Attack2State : CombatAttackState
    {
        public Attack2State
            (PlayerController player, PlayerStateMachine stateMachine, string animName)
            : base(player, stateMachine, animName) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Attack2");
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (Input.GetMouseButtonDown(0) && GetNormalizedTime() >= 0.6f && comboPossible == false)
            {
                comboPossible = true;
            }
            
            // 애니메이션이 80% 이상 진행되었을 때 다음 상태 결정
            if (GetNormalizedTime() >= 0.8f)
            {
                if (comboPossible)
                    stateMachine.ChangeState(player.attack3State); // 3타로 전이
                else
                    stateMachine.ChangeState(player.combatIdleState); // 입력 없으면 대기
            }
        }
        
        public override void Exit()
        {
            base.Exit();
            //player.ani.ResetTrigger(triggerName);
        }
    }
}