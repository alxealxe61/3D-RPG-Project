using _01._Script.Enemy.Range_Enemy.Bullet;
using _01._Script.StatePattern;

namespace _01._Script.Enemy.Boss_Enemy.Boss_Enemy_State.CombatState.Pattern2
{
    public class Pattern2Attack2 : BossState
    {
        private bool _hasFired;
        
        protected internal Pattern2Attack2
            (BossController owner, StateMachine<BossController> stateMachine, string aniName) 
            : base(owner, stateMachine, aniName) { }

        protected internal override void Enter()
        {
            base.Enter();   
            _hasFired = false;
        }

        protected internal override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!_hasFired && GetNormalizedTime() >= 0.1f)
            {
                BossEnemy.fireObject.SetActive(false);
                FireBullet();
                _hasFired = true;
            }
            
            if (GetNormalizedTime() >= 0.9f)
            {
                StateMachine.ChangeState(BossEnemy.BossIdleState);
            }
        }

        private void FireBullet()
        {
            if (Owner.Target == null) return;
            
            var bullet = BossEnemy.bulletPool.Get();
            bullet.Initialize(Owner.bossStats);
            bullet.transform.position = Owner.firePoint.position;
            bullet.Launch(Owner.Target.position);
        }
    }
}