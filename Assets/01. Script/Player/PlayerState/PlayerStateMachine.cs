using _01._Script.StataPattern;
using UnityEngine;

namespace _01._Script
{
    public class PlayerStateMachine : StateMachine<PlayerController>
    {
        public override void ChangeState(State<PlayerController> newState)
        {
            base.ChangeState(newState);
            
            Debug.Log($"플레이어 상태 변경: {newState.GetType().Name}");
        }
    }
}