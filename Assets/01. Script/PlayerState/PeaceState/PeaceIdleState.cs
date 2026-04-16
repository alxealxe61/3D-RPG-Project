using UnityEngine;

namespace _01._Script
{
    public class PeaceIdleState : PlayerState
    {
        private const float DODGE_DURATION_THRESHOLD = 0.9f;
        
        public PeaceIdleState
            (PlayerController player, PlayerStateMachine stateMachine, string animName) 
            : base(player, stateMachine, animName)
        { }
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();

            // WASD 이동 입력 시 PeaceMoveState로 전환
            if (player.InputVector.sqrMagnitude > 0)
            {
                stateMachine.ChangeState(player.peaceMoveState);
            }

            // 버그 때문에 이상하면 이거 끌수도 있음 
            if (Input.GetMouseButtonDown(0))
            {
                stateMachine.ChangeState(player.enterCombatState);
            }
            
            if (player.lockOnSystem.IsLockedOn == true)
            {
                stateMachine.ChangeState(player.enterCombatState);
            }
        }
    }
}