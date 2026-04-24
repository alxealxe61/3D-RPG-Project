using _01._Script.StataPattern;

namespace _01._Script.Enemy.Boss_Enemy.Boss_Enemy_State.CombatState.Pattern3
{
    public class Pattern3Attack1 : BossState
    {
        public Pattern3Attack1
            (BossController owner, StateMachine<BossController> stateMachine, string aniName) 
            : base(owner, stateMachine, aniName) { }

        public override void Enter()
        {
            base.Enter();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (GetNormalizedTime() >= 0.9f)
            {
                stateMachine.ChangeState(BossEnemy.Pattern3Attack2);
            }
        }

        public override void Exit()
        {
            base.Exit();
            BossEnemy.pHitBox.DisableDetection();
        }
    }
}