using UnityEngine;

namespace _01._Script.Enemy.Range_Enemy.Range_EnemyState.CombatState.pattern1
{
    public class Pattern1Attack1 : RangeEnemyState
    {
        private bool hasFired;
        
        public Pattern1Attack1
            (RangeEnemyController owner, RangeEnemyStateMachine stateMachine, string aniName, bool useBool) 
            : base(owner, stateMachine, aniName, useBool) { }

        public override void Enter()
        {
            base.Enter();   
            hasFired = false;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!hasFired && GetNormalizedTime() >= 0.5f)
            {
                FireBullet();
                hasFired = true;
            }
            
            if (GetNormalizedTime() >= 0.9f)
            {
                stateMachine.ChangeState(RangeEnemy.RangeCombatIdleState);
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
            stateMachine.ChangeState(RangeEnemy.RangeCombatIdleState);
        }
    }
}