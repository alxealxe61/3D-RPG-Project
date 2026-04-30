using _01._Script.StataPattern;
using UnityEngine;

namespace _01._Script.Enemy.Boss_Enemy.Boss_Enemy_State.CombatState
{
    public class BossStunState : BossState
    {
        private float stunDuration = 0.1f;
        private float timer;
        
        public BossStunState
            (BossController owner, StateMachine<BossController> stateMachine, string aniName) 
            : base(owner, stateMachine, aniName) { }
        
        public override void Enter()
        {
            base.Enter();
            timer = 0.0f;
            BossEnemy.fireObject.SetActive(false);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            timer += Time.deltaTime;
            
            if (timer >= stunDuration)
            {
                stateMachine.ChangeState(BossEnemy.BossIdleState);
            }
        }
    }
}