using _01._Script.Enemy.Range_Enemy;
using _01._Script.StataPattern;

namespace _01._Script.Enemy.Boss_Enemy.Boss_Enemy_State.CombatState.Pattern2
{
    public class Pattern2Attack2 : BossState
    {
        private bool hasFired;
        
        public Pattern2Attack2
            (BossController owner, StateMachine<BossController> stateMachine, string aniName) 
            : base(owner, stateMachine, aniName) { }
        
        public override void Enter()
        {
            base.Enter();   
            hasFired = false;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!hasFired && GetNormalizedTime() >= 0.1f)
            {
                BossEnemy.fireObject.SetActive(false);
                FireBullet();
                hasFired = true;
            }
            
            if (GetNormalizedTime() >= 0.9f)
            {
                stateMachine.ChangeState(BossEnemy.BossIdleState);
            }
        }

        private void FireBullet()
        {
            if (owner.Target == null) return;
            
            Bullet bullet = BulletPool.Instance.Get();
            bullet.transform.position = owner.firePoint.position;
            bullet.Launch(owner.Target.position);
        }
    }
}