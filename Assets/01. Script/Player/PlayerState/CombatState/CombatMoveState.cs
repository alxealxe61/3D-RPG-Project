using UnityEngine;

namespace _01._Script
{
    public class CombatMoveState : PlayerState
    {
        
        private const float SPRINT_MULTIPLIER = 2f;
        private const float ACCELERATION_SPEED = 2.0f; 
        
        private float currentSpeedMultiplier = 1.0f;

        public Vector2 GoalInput { get; private set; }
        
        public CombatMoveState
            (PlayerController player, PlayerStateMachine stateMachine, string animName, bool userBool) 
            : base(player, stateMachine, animName) { }
        
        public override void Enter()
        {
            base.Enter();
            currentSpeedMultiplier = 1.0f;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (player.InputVector.sqrMagnitude == 0)
            {
                stateMachine.ChangeState(player.CombatIdleState);
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                stateMachine.ChangeState(player.Attack1State);
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                stateMachine.ChangeState(player.CombatGuardState);
            }
            
            if (Input.GetKeyDown(KeyCode.LeftShift) && player.isDodge == false)
            {
                stateMachine.ChangeState(player.CombatDodgeState);
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                player.AttemptSkillUse();
            }
            
            if (player.lockOnSystem.IsLockedOn == false)
            {
                stateMachine.ChangeState(player.ExitCombatState);
            }
        }
        
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            
            Vector2 inputAxis = 
                new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            
            Vector2 currentAnimatorInput = new Vector2(player.ani.GetFloat("X"), player.ani.GetFloat("Y"));
            
            Vector2 applyInput = Vector2.Lerp(currentAnimatorInput, GoalInput, player.daming);
            
            Vector3 moveVector;

            // 록온 상태일 때의 이동 로직 (몬스터를 중심으로 공전)
            if (player.lockOnSystem != null && player.lockOnSystem.IsLockedOn && player.lockOnSystem.CurrentTarget != null)
            {
                Transform target = player.lockOnSystem.CurrentTarget;

                // 1. 타겟 방향 계산 (Y축 높이 차이 무시)
                Vector3 targetDir = (target.position - player.transform.position);
                targetDir.y = 0;
                targetDir.Normalize();

                // 2. 캐릭터가 항상 타겟을 바라보도록 회전
                if (targetDir != Vector3.zero)
                {
                    Quaternion targetRot = Quaternion.LookRotation(targetDir);
                    player.rb.MoveRotation(Quaternion.Slerp(player.rb.rotation, targetRot, Time.fixedDeltaTime * 10f));
                }

                // 3. 이동 벡터 계산 (타겟 기준, Y축 평면화)
                Vector3 targetRight = Vector3.Cross(Vector3.up, targetDir);
                moveVector = (targetDir * player.InputVector.y + targetRight * player.InputVector.x).normalized;
            }
            else
            {
                // 일반 이동 로직 (수평 평면 벡터 투영)
                Vector3 forward = player.transform.forward;
                Vector3 right = player.transform.right;
                forward.y = 0;
                right.y = 0;
                forward.Normalize();
                right.Normalize();

                moveVector = (forward * player.InputVector.y + right * player.InputVector.x).normalized;
            }

            // Rigidbody 속도 제어: Y축은 중력 영향을 위해 기존 velocity.y 유지
            float speed = player.moveSpeed / 2; // 전투 중 이동 속도 조정
            Vector3 targetVelocity = moveVector * speed;
            player.rb.linearVelocity = new Vector3(targetVelocity.x, player.rb.linearVelocity.y, targetVelocity.z);

            player.ani.SetFloat("X", applyInput.x);
            player.ani.SetFloat("Y", applyInput.y);
            }


        public override void Exit()
        {
            base.Exit();
            
            player.ani.SetFloat("X", 0);
            player.ani.SetFloat("Y", 0);
            
        }
        
        
    }
}