using _01._Script.StataPattern;

namespace _01._Script.Enemy.Boss_Enemy.Boss_Enemy_State.CombatState
{
    public class BossStunState : BossState
    {
        public BossStunState
            (BossController owner, StateMachine<BossController> stateMachine, string aniName) 
            : base(owner, stateMachine, aniName) { }
    }
}