using UnityEngine;

namespace _01._Script.Player.PlayerState.CombatState
{
    public class CombatIdleState : PlayerState
    {
        protected internal CombatIdleState
            (PlayerController player, PlayerStateMachine stateMachine, string animName)
            : base(player, stateMachine, animName) { }

        public override void Enter()
        {
            base.Enter();
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (Player.InputVector.sqrMagnitude > 0)
            {
                stateMachine.ChangeState(Player.CombatMoveState);
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                stateMachine.ChangeState(Player.Attack1State);
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                stateMachine.ChangeState(Player.CombatGuardState);
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && Player.isDodge == false)
            {
                stateMachine.ChangeState(Player.CombatDodgeState);
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Player.AttemptSkillUse();
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (Player.lockOnSystem == null || !Player.lockOnSystem.IsLockedOn ||
                Player.lockOnSystem.CurrentTarget == null) return;
            var targetDir = (Player.lockOnSystem.CurrentTarget.position - Player.transform.position);
            targetDir.y = 0;
            if (targetDir == Vector3.zero) return;
            var targetRot = Quaternion.LookRotation(targetDir);
            Player.transform.rotation = Quaternion.Slerp(Player.transform.rotation, targetRot, Time.deltaTime * 10f);
        }
    }
}