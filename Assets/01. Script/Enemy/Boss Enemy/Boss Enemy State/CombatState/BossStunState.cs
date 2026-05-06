using _01._Script.StatePattern;
using UnityEngine;

namespace _01._Script.Enemy.Boss_Enemy.Boss_Enemy_State.CombatState
{
    public class BossStunState : BossState
    {
        private float _timer;
        
        protected internal BossStunState
            (BossController owner, StateMachine<BossController> stateMachine, string aniName) 
            : base(owner, stateMachine, aniName) { }

        protected internal override void Enter()
        {
            base.Enter();
            _timer = 0.0f;
            BossEnemy.fireObject.SetActive(false);
        }

        protected internal override void LogicUpdate()
        {
            base.LogicUpdate();
            _timer += Time.deltaTime;
            
            if (_timer >= 0.1f)
            {
                StateMachine.ChangeState(BossEnemy.BossIdleState);
            }
        }
    }
}