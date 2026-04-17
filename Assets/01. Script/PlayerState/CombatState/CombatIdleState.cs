using UnityEngine;

namespace _01._Script
{
    public class CombatIdleState : PlayerState
    {
        public CombatIdleState
            (PlayerController player, PlayerStateMachine stateMachine, string animName)
            : base(player, stateMachine, animName) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Idle");
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (player.InputVector.sqrMagnitude > 0)
            {
                stateMachine.ChangeState(player.combatMoveState);
            }
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                stateMachine.ChangeState(player.attack1State);
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                stateMachine.BoolChangeState(player.combatGuardState);
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                stateMachine.ChangeState(player.combatDodgeState);
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                player.AttemptSkillUse();
            }
            
            if (player.lockOnSystem.IsLockedOn == false)
            { 
                stateMachine.ChangeState(player.exitCombatState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            // 록온 중일 때 캐릭터가 타겟을 부드럽게 바라보도록 유지
            if (player.lockOnSystem != null && player.lockOnSystem.IsLockedOn && player.lockOnSystem.CurrentTarget != null)
            {
                Vector3 targetDir = (player.lockOnSystem.CurrentTarget.position - player.transform.position);
                targetDir.y = 0;
                if (targetDir != Vector3.zero)
                {
                    Quaternion targetRot = Quaternion.LookRotation(targetDir);
                    player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRot, Time.deltaTime * 10f);
                }
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}