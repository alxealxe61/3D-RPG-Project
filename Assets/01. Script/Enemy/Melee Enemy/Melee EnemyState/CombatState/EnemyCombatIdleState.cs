using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _01._Script.Enemy.EnemyState.Melee_EnemyState.CombatState
{
    public class EnemyCombatIdleState : MeleeEnemyState
    {
        private List<MeleeEnemyState> patterns = new List<MeleeEnemyState>();

        private float idleTimer;
        private const float IDLE_TIME = 2.0f;
        
        public EnemyCombatIdleState
            (MeleeEnemyController owner, MeleeEnemyStateMachine stateMachine, string aniName, bool useBool)
            : base(owner, stateMachine, aniName, useBool)
        {
            patterns.Add(meleeEnemy.patten1Attack1);
            //Pattern.Add(typeof(Patten2Attack1));
        }

        public override void Enter()
        {
            base.Enter();
            idleTimer = 0f;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            // 2초 동안 공격 범위에 플레이어가 존재하면 2중 하나 패턴 랜덤 선택
            idleTimer += Time.deltaTime;
            Debug.Log(idleTimer);

            if (idleTimer >= IDLE_TIME)
            {
                stateMachine.ChangeState(meleeEnemy.patten1Attack1);
            }
        }

        private void ExecuteRandomPattern()
        {
            //if (patterns.Count == 0) return;
            
            int randNum = Random.Range(0, patterns.Count);
            MeleeEnemyState nextPattern = patterns[randNum];
            
            stateMachine.ChangeState(nextPattern);
        }
        
        public override void Exit()
        {
            base.Exit();
        }
    }
}