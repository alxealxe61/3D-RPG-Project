namespace _01._Script.Enemy.Range_Enemy.Range_EnemyState.CombatState.pattern1
{
    public class Pattern1Attack2 : RangeState
    {
        private bool hasFired;
        
        public Pattern1Attack2
            (RangeController owner, RangeStateMachine stateMachine, string aniName, bool useBool) 
            : base(owner, stateMachine, aniName, useBool) { }
        
        public override void Enter()
        {
            base.Enter();   
            hasFired = false;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!hasFired && GetNormalizedTime() >= 0.65f)
            {
                FireBullet();
                hasFired = true;
            }
            
            if (GetNormalizedTime() >= 0.9f)
            {
                stateMachine.ChangeState(RangeEnemy.RangeIdleState);
            }
        }

        private void FireBullet()
        {
            if (owner.Target == null) return;
            
            Bullet.Bullet bullet = RangeEnemy.bulletPool.Get();
            bullet.Initialize(owner.rangeStats);
            bullet.transform.position = owner.firePoint.position;
            bullet.Launch(owner.Target.position);
        }

        public override void Exit()
        {
            base.Exit();
            //RangeEnemy.bulletHitBox.DisableDetection();
        }
    }
}