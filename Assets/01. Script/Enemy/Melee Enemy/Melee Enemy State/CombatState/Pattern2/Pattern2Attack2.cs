using _01._Script.Enemy.Melee_Enemy;
using _01._Script.Enemy.Melee_Enemy.Melee_EnemyState;
using UnityEngine;

namespace _01._Script.Enemy.EnemyState.Melee_EnemyState.CombatState.Pattern2
{
    public class Pattern2Attack2 : MeleeState
    {
        private float nextAttack = 1f;
        private float timer;
        
        public Pattern2Attack2
            (MeleeController owner, MeleeStateMachine stateMachine, string aniName, bool useBool) 
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
                stateMachine.ChangeState(MeleeEnemy.Pattern2Attack3);
            }
        }
        
        public override void Exit()
        {
            base.Exit();
            MeleeEnemy.rHitBox.DisableDetection();
        }
    }
}