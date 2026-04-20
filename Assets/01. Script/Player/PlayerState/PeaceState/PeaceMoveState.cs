using UnityEngine;

namespace _01._Script
{
    public class PeaceMoveState : PlayerState
    {
        private const float SPRINT_MULTIPLIER = 2f;
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
                stateMachine.ChangeState(player.peaceIdleState);
            }
            
            if (player.lockOnSystem.IsLockedOn == true)
            {
                stateMachine.ChangeState(player.enterCombatState);
            }
        }
        
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            
            Vector2 inputAxis = 
                new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        
            if (Input.GetKey(KeyCode.LeftShift)) inputAxis *= 2.0f;
            

            GoalInput = inputAxis;

            //댐핑 먹였는데 뜻대로 안된다
            Vector2 currentAnimatorInput = new Vector2(player.ani.GetFloat("X"), player.ani.GetFloat("Y"));
            Vector2 applyInput = Vector2.Lerp(currentAnimatorInput, GoalInput, player.daming);
        
            player.ani.SetFloat("X", applyInput.x);
            player.ani.SetFloat("Y", applyInput.y);
        
            player.transform.Translate(new Vector3(inputAxis.x, 0, inputAxis.y) * player.moveSpeed * Time.deltaTime);
        }

        public override void Exit()
        {
            base.Exit();
            
            player.ani.SetFloat("X", 0);
            player.ani.SetFloat("Y", 0);
        }
    }
}