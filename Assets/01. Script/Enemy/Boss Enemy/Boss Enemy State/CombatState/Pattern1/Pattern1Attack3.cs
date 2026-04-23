using _01._Script.StataPattern;
using UnityEngine;

namespace _01._Script.Enemy.Boss_Enemy.Boss_Enemy_State.CombatState.Pattern1
{
    public class Pattern1Attack3 : BossState
    {
        private float nextAttack = 1f;
        private float timer;
        public Pattern1Attack3
            (BossController owner, StateMachine<BossController> stateMachine, string aniName)
            : base(owner, stateMachine, aniName) { }
        public override void Enter()
        {
            base.Enter();
            Debug.Log("Attack3");
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (GetNormalizedTime() >= 0.9f)
            {
                stateMachine.ChangeState(BossEnemy.BossIdleState);
            }
        }
        
        public override void Exit()
        {
            base.Exit();
            BossEnemy.rHitBox.DisableDetection();
            BossEnemy.rHitBox.DisableDetection();
        }
    }
}