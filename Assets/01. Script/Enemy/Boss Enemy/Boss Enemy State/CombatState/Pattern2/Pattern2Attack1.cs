using _01._Script.StataPattern;
using Unity.VisualScripting;

namespace _01._Script.Enemy.Boss_Enemy.Boss_Enemy_State.CombatState.Pattern2
{
    public class Pattern2Attack1 : BossState
    {
        public Pattern2Attack1
            (BossController owner, StateMachine<BossController> stateMachine, string aniName)
            : base(owner, stateMachine, aniName) { }

        public override void Enter()
        {
            base.Enter();
            
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (GetNormalizedTime() >= 0.3f)
            {
                BossEnemy.fireObject.SetActive(true);
            }
            
            if (GetNormalizedTime() >= 0.9f)
            { 
                stateMachine.ChangeState(BossEnemy.Pattern2Attack2);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}