using UnityEngine;

namespace _01._Script
{
    public class CombatSkillState : PlayerState
    {
        
        public CombatSkillState
            (PlayerController player, PlayerStateMachine stateMachine, string animName) 
            : base(player, stateMachine, animName) { }

        public override void Enter()
        {
            base.Enter();
            // 여기서 스킬 게이지 차감 하고
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (GetNormalizedTime() >= 0.9f)
            {
                if (player.InputVector.sqrMagnitude > 0.1f)
                {
                    stateMachine.ChangeState(player.combatMoveState);
                }
                else
                {
                    stateMachine.ChangeState(player.combatIdleState);
                }
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("Exit CombatSkillState");
            // 입력이 있다면 이동 상태로, 없다면 대기 상태로 전환합니다.
            
        }
    }
}