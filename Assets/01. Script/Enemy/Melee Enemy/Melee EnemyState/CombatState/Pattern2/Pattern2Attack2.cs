using UnityEngine;

namespace _01._Script.Enemy.EnemyState.Melee_EnemyState.CombatState.Pattern2
{
    public class Pattern2Attack2 : MeleeEnemyState
    {
        private float nextAttack = 1f;
        private float timer;
        
        public Pattern2Attack2
            (MeleeEnemyController owner, MeleeEnemyStateMachine stateMachine, string aniName, bool useBool) 
            : base(owner, stateMachine, aniName, useBool) { }
        
        public override void Enter()
        {
            base.Enter();
            timer = 0f;
        }

        public override void LogicUpdate()
        {
            timer += Time.deltaTime;
            base.LogicUpdate();
            //플레이어의 스킬에 맞으면 스턴 상태로 돌아가는 함수 호출 
            if (timer >= nextAttack)
            {
                stateMachine.ChangeState(meleeEnemy.Pattern2Attack3);
            }
        }
        
        public override void Exit()
        {
            base.Exit();
            meleeEnemy.rHitBox.DisableDetection();
        }
    }
}