using UnityEngine;

namespace _01._Script
{
    public class CombatMoveState : PlayerState
    {
        
        private const float SPRINT_MULTIPLIER = 2f;
        private const float ACCELERATION_SPEED = 2.0f; 
        
        private float currentSpeedMultiplier = 1.0f;

        public Vector2 InputVector { get; private set; }
        
        public CombatMoveState
            (PlayerController player, PlayerStateMachine stateMachine, string animName) 
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
                stateMachine.ChangeState(player.combatIdleState);
            }
            
            if (Input.GetMouseButtonDown(0))
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
                stateMachine.ChangeState(player.combatSkillState);
            }
        }
        
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            InputVector = new Vector2(x, y).normalized;
            
            Vector3 moveVector = (player.transform.forward * player.InputVector.y + player.transform.right * player.InputVector.x).normalized;
            player.transform.position += moveVector * (player.moveSpeed * currentSpeedMultiplier * Time.deltaTime);
            
            player.ani.SetFloat("X", InputVector.x * currentSpeedMultiplier);
            player.ani.SetFloat("Y", InputVector.y * currentSpeedMultiplier);
        }

        public override void Exit()
        {
            base.Exit();
            
            player.ani.SetFloat("X", 0);
            player.ani.SetFloat("Y", 0);
            
        }
        
        
    }
}