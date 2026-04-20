namespace _01._Script.Enemy.EnemyState.Melee_EnemyState.CombatState.AttackPatten1
{
    public class Patten1Attack1 : MeleeEnemyState
    {
        public Patten1Attack1
            (MeleeEnemyController owner, MeleeEnemyStateMachine stateMachine, string aniName, bool useBool) 
            : base(owner, stateMachine, aniName, useBool) { }
        
        public override void Enter()
        {
            base.Enter();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            //플레이어의 스킬에 맞으면 스턴 상태로 돌아가는 함수 호출 
            if (GetNormalizedTime() >= 0.9f)
            {
                stateMachine.ChangeState(meleeEnemy.patten1Attack2);
            }
        }
        
        public override void Exit()
        {
            base.Exit();
        }
    }
}