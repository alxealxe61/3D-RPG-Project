using _01._Script.StataPattern;
using UnityEngine;

namespace _01._Script.Enemy.Boss_Enemy.Boss_Enemy_State.CombatState
{
    public class BossDieState : BossState
    {
        public BossDieState
            (BossController owner, StateMachine<BossController> stateMachine, string aniName) 
            : base(owner, stateMachine, aniName) { }
        
        public override void Enter()
        {
            base.Enter();
            Debug.Log("Die");
            BossEnemy.bossStats.Die();
            BossEnemy.IsDie();
        }
    }
}