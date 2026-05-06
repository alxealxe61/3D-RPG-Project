using _01._Script.StatePattern;

namespace _01._Script.Enemy.Boss_Enemy.Boss_Enemy_State.CombatState.Pattern3
{
    public class Pattern3Attack2 : BossState
    {
        protected internal Pattern3Attack2
            (BossController owner, StateMachine<BossController> stateMachine, string aniName) 
            : base(owner, stateMachine, aniName) { }

        protected internal override void LogicUpdate()
        {
            base.LogicUpdate();
            if (GetNormalizedTime() >= 0.9f)
            {
                StateMachine.ChangeState(BossEnemy.BossIdleState);
            }
        }

        protected internal override void Exit()
        {
            base.Exit();
            BossEnemy.rHitBox.DisableDetection();
            BossEnemy.lHitBox.DisableDetection();
        }
    }
}