using _01._Script.StatePattern;

namespace _01._Script.Player.PlayerState.CombatState
{
    public class CombatDieState : PlayerState
    {
        protected internal CombatDieState
            (PlayerController player, StateMachine<PlayerController> stateMachine, string animName) 
            : base(player, stateMachine, animName) { }
    }
}