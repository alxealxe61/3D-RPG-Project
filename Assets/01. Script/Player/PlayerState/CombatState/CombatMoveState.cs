using UnityEngine;

namespace _01._Script.Player.PlayerState.CombatState
{
    public class CombatMoveState : PlayerState
    {
        private static readonly int X = Animator.StringToHash("X");
        private static readonly int Y = Animator.StringToHash("Y");
        
        private Vector2 GoalInput { get; set; }
        
        protected internal CombatMoveState
            (PlayerController player, PlayerStateMachine stateMachine, string animName) 
            : base(player, stateMachine, animName) { }

        protected internal override void LogicUpdate()
        {
            base.LogicUpdate();

            if (Player.InputVector.sqrMagnitude == 0)
            {
                StateMachine.ChangeState(Player.CombatIdleState);
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                StateMachine.ChangeState(Player.Attack1State);
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                StateMachine.ChangeState(Player.CombatGuardState);
            }
            
            if (Input.GetKeyDown(KeyCode.LeftShift) && Player.isDodge == false)
            {
                StateMachine.ChangeState(Player.CombatDodgeState);
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Player.AttemptSkillUse();
            }
            
            if (Player.lockOnSystem.IsLockedOn == false)
            {
                StateMachine.ChangeState(Player.ExitCombatState);
            }
        }

        protected internal override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            var currentAnimatorInput = new Vector2(Player.ani.GetFloat(X), Player.ani.GetFloat(Y));
            
            var applyInput = Vector2.Lerp(currentAnimatorInput, GoalInput, Player.daming);
            
            Vector3 moveVector;

            // 록온 상태일 때의 이동 로직 (몬스터를 중심으로 공전)
            if (Player.lockOnSystem != null && Player.lockOnSystem.IsLockedOn && Player.lockOnSystem.CurrentTarget != null)
            {
                var target = Player.lockOnSystem.CurrentTarget;

                // 1. 타겟 방향 계산 (Y축 높이 차이 무시)
                var targetDir = (target.position - Player.transform.position);
                targetDir.y = 0;
                targetDir.Normalize();

                // 2. 캐릭터가 항상 타겟을 바라보도록 회전
                if (targetDir != Vector3.zero)
                {
                    Quaternion targetRot = Quaternion.LookRotation(targetDir);
                    Player.rb.MoveRotation(Quaternion.Slerp(Player.rb.rotation, targetRot, Time.fixedDeltaTime * 10f));
                }

                // 3. 이동 벡터 계산 (타겟 기준, Y축 평면화)
                var targetRight = Vector3.Cross(Vector3.up, targetDir);
                moveVector = (targetDir * Player.InputVector.y + targetRight * Player.InputVector.x).normalized;
            }
            else
            {
                // 일반 이동 로직 (수평 평면 벡터 투영)
                var forward = Player.transform.forward;
                var right = Player.transform.right;
                forward.y = 0;
                right.y = 0;
                forward.Normalize();
                right.Normalize();

                moveVector = (forward * Player.InputVector.y + right * Player.InputVector.x).normalized;
            }

            // Rigidbody 속도 제어: Y축은 중력 영향을 위해 기존 velocity.y 유지
            var speed = Player.MoveSpeed / 2; // 전투 중 이동 속도 조정
            var targetVelocity = moveVector * speed;
            Player.rb.linearVelocity = new Vector3(targetVelocity.x, Player.rb.linearVelocity.y, targetVelocity.z);

            Player.ani.SetFloat(X, applyInput.x);
            Player.ani.SetFloat(Y, applyInput.y);
            }


        protected internal override void Exit()
        {
            base.Exit();
            
            Player.ani.SetFloat(X, 0);
            Player.ani.SetFloat(Y, 0);
        }
        
        
    }
}