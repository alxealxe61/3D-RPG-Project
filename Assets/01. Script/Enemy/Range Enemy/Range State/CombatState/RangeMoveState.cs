namespace _01._Script.Enemy.Range_Enemy.Range_EnemyState.CombatState
{
    public class RangeMoveState : RangeState
    {
        public RangeMoveState
            (RangeController owner, RangeStateMachine stateMachine, string aniName,bool useBool) 
            : base(owner, stateMachine, aniName, useBool) { }
        
        public override void Enter()
        {
            base.Enter();
            if (Agent != null && Agent.isOnNavMesh)
            {
                Agent.isStopped = false;
                Agent.speed = owner.MoveSpeed;
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (owner.Target != null && Agent != null && Agent.isOnNavMesh)
            {
                Agent.SetDestination(owner.Target.position);
            }
        }

        
        public override void Exit()
        {
            base.Exit();
            if (Agent != null && Agent.isOnNavMesh)
            {
                Agent.isStopped = true;
            }
        }
    }
}