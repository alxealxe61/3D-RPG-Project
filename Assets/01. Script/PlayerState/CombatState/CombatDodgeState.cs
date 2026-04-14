using UnityEngine;

namespace _01._Script
{
    public class CombatDodgeState : PlayerState
    {
        private const float DODGE_SPEED = 10.0f; // 회피 시 뒤로 물러나는 속도
        private const float DODGE_DURATION_THRESHOLD = 0.9f; // 애니메이션 재생 완료 임계값 (0.9 = 90%)

        public CombatDodgeState
            (PlayerController player, PlayerStateMachine stateMachine, string animName) 
            : base(player, stateMachine, animName) { }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            // 애니메이션이 거의 끝날 때까지 다른 상태로의 전환을 차단합니다.
            if (GetNormalizedTime() >= DODGE_DURATION_THRESHOLD)
            {
                // 입력이 있다면 이동 상태로, 없다면 대기 상태로 전환합니다.
                if (player.InputVector.sqrMagnitude > 0.01f)
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

            // 회피 동작 중(애니메이션 초반부)에만 캐릭터를 뒤로 이동시킵니다.
            if (GetNormalizedTime() < 0.7f)
            {
                player.transform.position -= player.transform.forward * (DODGE_SPEED * Time.deltaTime);
            }
        }
    }
}
