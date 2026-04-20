namespace _01._Script.Enemy.EnemyState.Melee_EnemyState.PeaceState
{
    public class EnemyPeaceIdleState : MeleeEnemyState
    {
        public EnemyPeaceIdleState
            (MeleeEnemyController owner, MeleeEnemyStateMachine stateMachine, string aniName, bool useBool)
            : base(owner, stateMachine, aniName, useBool) { }
    }
}