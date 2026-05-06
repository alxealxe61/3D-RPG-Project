using _01._Script.StatePattern;

namespace _01._Script.Enemy.Boss_Enemy.Boss_Enemy_State.CombatState.Pattern2
{
    public class Pattern2Attack1 : BossState
    {
        protected internal Pattern2Attack1
            (BossController owner, StateMachine<BossController> stateMachine, string aniName)
            : base(owner, stateMachine, aniName) { }

        protected internal override void LogicUpdate()
        {
            base.LogicUpdate();

            if (GetNormalizedTime() >= 0.3f)
            {
                BossEnemy.fireObject.SetActive(true);
            }
            
            if (GetNormalizedTime() >= 0.9f)
            { 
                StateMachine.ChangeState(BossEnemy.Pattern2Attack2);
            }
        }
    }
}