using _01._Script.StatePattern;
using UnityEngine;

namespace _01._Script.Enemy.Boss_Enemy.Boss_Enemy_State.CombatState
{
    public class BossDieState : BossState
    {
        protected internal BossDieState
            (BossController owner, StateMachine<BossController> stateMachine, string aniName) 
            : base(owner, stateMachine, aniName) { }

        protected internal override void Enter()
        {
            base.Enter();
            BossEnemy.bossStats.Die();
            BossEnemy.IsDie();
        }
    }
}