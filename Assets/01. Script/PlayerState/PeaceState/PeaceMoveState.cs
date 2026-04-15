using UnityEngine;

namespace _01._Script
{
    public class PeaceMoveState : PlayerState
    {
        private const float SPRINT_MULTIPLIER = 2f;
        private const float ACCELERATION_SPEED = 2.0f; 
        
        private float currentSpeedMultiplier = 1.0f;

        public Vector2 InputVector { get; private set; }
        
        public PeaceMoveState
            (PlayerController player, PlayerStateMachine stateMachine, string animName) 
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
            
            if (Input.GetMouseButtonDown(0))
            {
                player.isWeaponInHand = true;
                player.ani.SetTrigger("EnterCombatState");
                stateMachine.ChangeState(player.combatIdleState);
            }
        }
        
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            
            float targetMultiplier = Input.GetKey(KeyCode.LeftShift) ? SPRINT_MULTIPLIER : 1.0f;
            
            currentSpeedMultiplier = Mathf.Lerp(currentSpeedMultiplier, targetMultiplier, ACCELERATION_SPEED * Time.deltaTime);
            
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