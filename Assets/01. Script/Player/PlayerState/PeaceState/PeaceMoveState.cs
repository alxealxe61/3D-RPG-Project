using UnityEngine;

namespace _01._Script.Player.PlayerState.PeaceState
{
    public class PeaceMoveState : PlayerState
    {
        private static readonly int X = Animator.StringToHash("X");
        private static readonly int Y = Animator.StringToHash("Y");
        private const float SprintMultiplier = 1.2f;
        private const float AccelerationSpeed = 2.0f; 
        
        private Vector2 GoalInput { get; set; }
        
        protected internal PeaceMoveState
            (PlayerController player, PlayerStateMachine stateMachine, string animName) 
            : base(player, stateMachine, animName) { }
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (Player.InputVector.sqrMagnitude == 0)
            {
                stateMachine.ChangeState(Player.PeaceIdleState);
            }
            
            if (Player.lockOnSystem.IsLockedOn == true)
            {
                stateMachine.ChangeState(Player.EnterCombatState);
            }
        }
        
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            var inputAxis = Player.InputVector;
            var currentMoveSpeed = Player.MoveSpeed;

            var isSprinting = Input.GetKey(KeyCode.LeftShift);
            Player.ani.applyRootMotion = !isSprinting;
            
            if (isSprinting)
            {
                currentMoveSpeed *= SprintMultiplier;
                GoalInput = inputAxis * AccelerationSpeed;
            }
            else
            {
                GoalInput = inputAxis;
            }
            
            var currentAnimatorInput = new Vector2(Player.ani.GetFloat(X), Player.ani.GetFloat(Y));
            var applyInput = Vector2.Lerp(currentAnimatorInput, GoalInput, Player.daming);

            Player.ani.SetFloat(X, applyInput.x);
            Player.ani.SetFloat(Y, applyInput.y);
            
            var forward = Player.transform.forward;
            var right = Player.transform.right;
    
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();
            
            var moveDirection = (forward * inputAxis.y + right * inputAxis.x);
            
            if (moveDirection.magnitude > 0.1f)
            {
                moveDirection.Normalize();
                var targetVelocity = moveDirection * currentMoveSpeed;
                
                Player.rb.linearVelocity = new Vector3(targetVelocity.x, Player.rb.linearVelocity.y, targetVelocity.z);
            }
            else
            {
                Player.rb.linearVelocity = new Vector3(0, Player.rb.linearVelocity.y, 0);
            }

        }
        
        public override void Exit()
        {
            base.Exit();
            
            Player.ani.SetFloat(X, 0);
            Player.ani.SetFloat(Y, 0);
        }
    }
}