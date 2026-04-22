using _01._Script.Enemy.Melee_Enemy;
using _01._Script.Enemy.Melee_Enemy.Melee_EnemyState;

namespace _01._Script.Enemy.EnemyState.Melee_EnemyState.CombatState.AttackPatten1
{
    public class Pattern1Attack2 : MeleeEnemyState
    {
        public Pattern1Attack2
            (MeleeEnemyController owner, MeleeEnemyStateMachine stateMachine, string aniName, bool useBool)
            : base(owner, stateMachine, aniName, useBool) { }
        
        public override void Enter()
        {
            base.Enter();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (GetNormalizedTime() >= 0.9f)
            {
                //meleeEnemy.isAttacking = false;
                stateMachine.ChangeState(MeleeEnemy.CombatIdleState);
            }
        }
        
        public override void Exit()
        {
            base.Exit();
            MeleeEnemy.lHitBox.DisableDetection();
        }
    }
}