using UnityEngine;

namespace _01._Script.Enemy.Melee_Enemy.Melee_Enemy_State.CombatState
{
    public class MeleeDieState : MeleeState
    {
        protected internal MeleeDieState
            (MeleeController owner, MeleeStateMachine stateMachine, string aniName) 
            : base(owner, stateMachine, aniName) { }

        protected internal override void Enter()
        {
            base.Enter();
            
            MeleeEnemy.meleeStats.Die();
            MeleeEnemy.IsDie();
        }
    }
}