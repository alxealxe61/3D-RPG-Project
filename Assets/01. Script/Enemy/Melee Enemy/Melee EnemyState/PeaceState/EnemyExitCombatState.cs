namespace _01._Script.Enemy.EnemyState.Melee_EnemyState.PeaceState
{
    public class EnemyExitCombatState : MeleeEnemyState
    {
        public EnemyExitCombatState
            (MeleeEnemyController owner, MeleeEnemyStateMachine stateMachine, string aniName, bool useBool)
            : base(owner, stateMachine, aniName, useBool) { }
    }
}