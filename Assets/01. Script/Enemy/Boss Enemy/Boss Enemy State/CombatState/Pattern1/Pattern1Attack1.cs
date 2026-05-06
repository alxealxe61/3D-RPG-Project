using _01._Script.StatePattern;
using UnityEngine;

namespace _01._Script.Enemy.Boss_Enemy.Boss_Enemy_State.CombatState.Pattern1
{
    public class Pattern1Attack1 : BossState
    {
        protected internal Pattern1Attack1
            (BossController owner, StateMachine<BossController> stateMachine, string aniName)
            : base(owner, stateMachine, aniName) { }

        protected internal override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (GetNormalizedTime() >= 0.9f)
            {
                StateMachine.ChangeState(BossEnemy.Pattern1Attack2);
            }
        }

        protected internal override void Exit()
        {
            base.Exit();
            BossEnemy.lHitBox.DisableDetection();
        }
    }
}