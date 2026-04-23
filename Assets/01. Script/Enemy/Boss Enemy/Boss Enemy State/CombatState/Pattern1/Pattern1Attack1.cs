using _01._Script.StataPattern;
using UnityEngine;

namespace _01._Script.Enemy.Boss_Enemy.Boss_Enemy_State.CombatState.Pattern1
{
    public class Pattern1Attack1 : BossState
    {
        public Pattern1Attack1
            (BossController owner, StateMachine<BossController> stateMachine, string aniName)
            : base(owner, stateMachine, aniName) { }
        
        public override void Enter()
        {
            base.Enter();
            Debug.Log("Attack1");
        }
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (GetNormalizedTime() >= 0.9f)
            {
                stateMachine.ChangeState(BossEnemy.Pattern1Attack2);
            }
        }
        
        public override void Exit()
        {
            base.Exit();
            BossEnemy.lHitBox.DisableDetection();
        }
    }
}