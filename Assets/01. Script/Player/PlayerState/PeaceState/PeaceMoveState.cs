using UnityEngine;

namespace _01._Script
{
    public class PeaceMoveState : PlayerState
    {
        private const float SPRINT_MULTIPLIER = 1.2f;
        private const float ACCELERATION_SPEED = 2.0f; 
        
        private float currentSpeedMultiplier = 1.0f;

        public Vector2 GoalInput { get; private set; }
        
        public PeaceMoveState
            (PlayerController player, PlayerStateMachine stateMachine, string animName, bool userBool) 
            : base(player, stateMachine, animName)
        { }

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
                stateMachine.ChangeState(player.PeaceIdleState);
            }
            
            if (player.lockOnSystem.IsLockedOn == true)
            {
                stateMachine.ChangeState(player.EnterCombatState);
            }
        }
        
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            Vector2 inputAxis = player.InputVector;
            float currentMoveSpeed = player.moveSpeed;

            bool isSprinting = Input.GetKey(KeyCode.LeftShift);
            player.ani.applyRootMotion = !isSprinting;
            
            if (isSprinting)
            {
                currentMoveSpeed *= SPRINT_MULTIPLIER;
                GoalInput = inputAxis * ACCELERATION_SPEED;
            }
            else
            {
                GoalInput = inputAxis;
            }
            
            Vector2 currentAnimatorInput = new Vector2(player.ani.GetFloat("X"), player.ani.GetFloat("Y"));
            Vector2 applyInput = Vector2.Lerp(currentAnimatorInput, GoalInput, player.daming);

            player.ani.SetFloat("X", applyInput.x);
            player.ani.SetFloat("Y", applyInput.y);
    
            // 2. 방향 벡터 계산 (Y축 완전 차단)
            Vector3 forward = player.transform.forward;
            Vector3 right = player.transform.right;
    
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();
    
            // 3. 이동 방향 계산 (입력 벡터의 방향만 추출)
            // inputAxis.y는 앞뒤, x는 좌우 이동을 결정합니다.
            Vector3 moveDirection = (forward * inputAxis.y + right * inputAxis.x);

            // 입력이 있을 때만 정규화하여 방향을 잡고 속도를 곱함
            if (moveDirection.magnitude > 0.1f)
            {
                moveDirection.Normalize();
                Vector3 targetVelocity = moveDirection * currentMoveSpeed;
        
                // 4. 최종 속도 적용 (Y축은 기존 물리 속도-중력-를 절대 건드리지 않음)
                player.rb.linearVelocity = new Vector3(targetVelocity.x, player.rb.linearVelocity.y, targetVelocity.z);
            }
            else
            {
                // 입력이 없을 때는 X, Z 속도만 0으로 (미끄러짐 방지)
                player.rb.linearVelocity = new Vector3(0, player.rb.linearVelocity.y, 0);
            }

        }
        
        public override void Exit()
        {
            base.Exit();
            
            player.ani.SetFloat("X", 0);
            player.ani.SetFloat("Y", 0);
        }
    }
}