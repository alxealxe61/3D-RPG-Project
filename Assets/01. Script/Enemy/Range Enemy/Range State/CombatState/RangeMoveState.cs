namespace _01._Script.Enemy.Range_Enemy.Range_State.CombatState
{
    public class RangeMoveState : RangeState
    {
        protected internal RangeMoveState
            (RangeController owner, RangeStateMachine stateMachine, string aniName) 
            : base(owner, stateMachine, aniName) { }

        protected internal override void Enter()
        {
            base.Enter();
            if (Agent != null && Agent.isOnNavMesh)
            {
                Agent.isStopped = false;
                Agent.speed = Owner.MoveSpeed;
            }
        }

        protected internal override void LogicUpdate()
        {
            base.LogicUpdate();

            if (Owner.Target != null && Agent != null && Agent.isOnNavMesh)
            {
                Agent.SetDestination(Owner.Target.position);
            }
        }


        protected internal override void Exit()
        {
            base.Exit();
            if (Agent != null && Agent.isOnNavMesh)
            {
                Agent.isStopped = true;
            }
        }
    }
}