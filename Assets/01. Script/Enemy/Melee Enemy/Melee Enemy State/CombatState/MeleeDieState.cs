using _01._Script.Enemy.Melee_Enemy;
using _01._Script.Enemy.Melee_Enemy.Melee_EnemyState;
using UnityEngine;

namespace _01._Script.Enemy.EnemyState.Melee_EnemyState.CombatState
{
    public class MeleeDieState : MeleeState
    {
        public MeleeDieState
            (MeleeController owner, MeleeStateMachine stateMachine, string aniName, bool useBool) 
            : base(owner, stateMachine, aniName, useBool) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Die");
            MeleeEnemy.meleeStats.Die();
            MeleeEnemy.IsDie();
        }
    }
}