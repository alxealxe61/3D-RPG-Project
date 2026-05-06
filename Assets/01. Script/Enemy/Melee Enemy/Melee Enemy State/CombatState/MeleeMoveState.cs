namespace _01._Script.Enemy.Melee_Enemy.Melee_Enemy_State.CombatState
{
    public class MeleeMoveState : MeleeState
    {
        protected internal MeleeMoveState
            (MeleeController owner, MeleeStateMachine stateMachine, string aniName)
            : base(owner, stateMachine, aniName) { }

        protected internal override void Enter()
        {
            base.Enter();
            if (Agent == null || !Agent.isOnNavMesh) return;
            Agent.isStopped = false;
            Agent.speed = Owner.MoveSpeed;
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