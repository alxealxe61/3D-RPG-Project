using UnityEngine;

namespace _01._Script.Enemy.Range_Enemy.Range_EnemyState.CombatState.pattern1
{
    public class Pattern1Attack1 : RangeState
    {
        private bool hasFired;
        
        public Pattern1Attack1
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
                stateMachine.ChangeState(RangeEnemy.Pattern1Attack2);
            }
        }

        private void FireBullet()
        {
            if (owner.Target == null) return;
            
            Bullet bullet = BulletPool.Instance.Get();
            bullet.transform.position = owner.firePoint.position;
            bullet.Launch(owner.Target.position);
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}