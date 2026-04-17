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
            
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            InputVector = new Vector2(x, y).normalized;

            Vector3 moveVector;

            // 록온 상태일 때의 이동 로직 (몬스터를 중심으로 공전)
            if (player.lockOnSystem != null && player.lockOnSystem.IsLockedOn && player.lockOnSystem.CurrentTarget != null)
            {
                Transform target = player.lockOnSystem.CurrentTarget;
                
                // 1. 타겟 방향 계산 (Y축 높이 차이 무시)
                Vector3 targetDir = (target.position - player.transform.position);
                targetDir.y = 0;
                targetDir.Normalize();

                // 2. 캐릭터가 항상 타겟을 바라보도록 회전 (공전의 기초)
                if (targetDir != Vector3.zero)
                {
                    Quaternion targetRot = Quaternion.LookRotation(targetDir);
                    player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRot, Time.deltaTime * 10f);
                }

                // 3. 이동 벡터 계산 (타겟 기준)
                // 전후 입력(Y): 타겟으로 접근/멀어짐
                // 좌우 입력(X): 타겟 기준 수직 방향으로 이동 (즉, 공전)
                Vector3 targetRight = Vector3.Cross(Vector3.up, targetDir);
                moveVector = (targetDir * player.InputVector.y + (targetRight) * player.InputVector.x).normalized;
            }
            else
            {
                // 일반 이동 로직
                moveVector = (player.transform.forward * player.InputVector.y + player.transform.right * player.InputVector.x).normalized;
            }
            
            player.transform.position += moveVector * (player.moveSpeed * Time.deltaTime);
            
            player.ani.SetFloat("X", InputVector.x);
            player.ani.SetFloat("Y", InputVector.y);
        }

        public override void Exit()
        {
            base.Exit();
            
            player.ani.SetFloat("X", 0);
            player.ani.SetFloat("Y", 0);
            
        }
        
        
    }
}