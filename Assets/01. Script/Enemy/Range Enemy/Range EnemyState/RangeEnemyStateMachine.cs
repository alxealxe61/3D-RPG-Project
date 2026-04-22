using System;
using _01._Script.StataPattern;
using UnityEngine;

namespace _01._Script.Enemy.Range_Enemy.Range_EnemyState
{
    public class RangeEnemyStateMachine : StateMachine<RangeEnemyController>
    {
        public override void ChangeState(State<RangeEnemyController> newState)
        {
            base.ChangeState(newState);
            
            //Debug.Log($"원거리 몬스터 상태 변경: {newState.GetType().Name}");
        }
    }
}