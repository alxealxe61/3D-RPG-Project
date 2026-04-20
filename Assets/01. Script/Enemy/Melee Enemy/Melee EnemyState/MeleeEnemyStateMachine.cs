using _01._Script.StataPattern;
using UnityEngine;

namespace _01._Script.Enemy.EnemyState.Melee_EnemyState
{
    public class MeleeEnemyStateMachine : StateMachine<MeleeEnemyController>
    {
        public override void ChangeState(State<MeleeEnemyController> newState)
        {
            base.ChangeState(newState);
            
            Debug.Log($"플레이어 상태 변경: {newState.GetType().Name}");
        }
    }
}