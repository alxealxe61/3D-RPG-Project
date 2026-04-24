using UnityEngine;

namespace _01._Script
{
    public class PeaceIdleState : PlayerState
    {
        //private const float DODGE_DURATION_THRESHOLD = 0.9f;
        
        public PeaceIdleState
            (PlayerController player, PlayerStateMachine stateMachine, string animName, bool userBool) 
            : base(player, stateMachine, animName)
        { }
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();

            // WASD 이동 입력 시 PeaceMoveState로 전환
            if (player.InputVector.sqrMagnitude > 0)
            {
                stateMachine.ChangeState(player.PeaceMoveState);
            }
            
            if (player.lockOnSystem.IsLockedOn == true)
            {
                stateMachine.ChangeState(player.EnterCombatState);
            }
        }
    }
}