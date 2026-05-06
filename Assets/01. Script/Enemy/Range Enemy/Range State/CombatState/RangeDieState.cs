namespace _01._Script.Enemy.Range_Enemy.Range_State.CombatState
{
    public class RangeDieState : RangeState
    {
        protected internal RangeDieState
            (RangeController owner, RangeStateMachine stateMachine, string aniName) 
            : base(owner, stateMachine, aniName) { }

        protected internal override void Enter()
        {
            base.Enter();
            RangeEnemy.rangeStats.Die();
            RangeEnemy.IsDie();
        }
    }
}