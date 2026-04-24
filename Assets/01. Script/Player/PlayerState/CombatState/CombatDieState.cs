using _01._Script.StataPattern;
using UnityEngine;

namespace _01._Script
{
    public class CombatDieState : PlayerState
    {
        public CombatDieState
            (PlayerController player, StateMachine<PlayerController> stateMachine, string animName, bool useBool = false) 
            : base(player, stateMachine, animName, useBool) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Die");
        }
    }
}