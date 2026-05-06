namespace _01._Script.Enemy.Range_Enemy.Range_State.CombatState.Pattern1
{
    public class Pattern1Attack1 : RangeState
    {
        private bool _hasFired;
        
        public Pattern1Attack1
            (RangeController owner, RangeStateMachine stateMachine, string aniName) 
            : base(owner, stateMachine, aniName) { }

        protected internal override void Enter()
        {
            base.Enter();   
            _hasFired = false;
        }

        protected internal override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!_hasFired && GetNormalizedTime() >= 0.65f)
            {
                FireBullet();
                _hasFired = true;
            }
            
            if (GetNormalizedTime() >= 0.9f)
            {
                StateMachine.ChangeState(RangeEnemy.Pattern1Attack2);
            }
        }

        private void FireBullet()
        {
            if (Owner.Target == null) return;

            var bullet = RangeEnemy.bulletPool.Get();
            bullet.Initialize(Owner.rangeStats);
            bullet.transform.position = Owner.firePoint.position;
            bullet.Launch(Owner.Target.position);
        }
    }
}