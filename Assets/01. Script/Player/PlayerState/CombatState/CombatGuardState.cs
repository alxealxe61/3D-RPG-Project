using UnityEngine;

namespace _01._Script
{
    public class CombatGuardState : PlayerState
    {
        public CombatGuardState
            (PlayerController player, PlayerStateMachine stateMachine, string animName, bool userBool)
            : base(player, stateMachine, animName) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Combat Guard State");
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            // 여기에 일반 가드와 퍼펙트 가드 판정이 들어감
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (Input.GetMouseButtonUp(1))
            {
                stateMachine.ChangeState(player.combatIdleState);
            }
        }
    }
}