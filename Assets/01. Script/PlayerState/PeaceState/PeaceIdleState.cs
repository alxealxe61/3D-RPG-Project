using UnityEngine;

namespace _01._Script
{
    public class PeaceIdleState : PlayerState
    {
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

            // 마우스 왼쪽 클릭 시 즉시 공격 상태로 전환
            if (Input.GetMouseButtonDown(0))
            {
                player.isWeaponInHand = true;
                player.ani.SetTrigger("EnterCombatState");
                stateMachine.ChangeState(player.combatIdleState);
            }
        }
    }
}