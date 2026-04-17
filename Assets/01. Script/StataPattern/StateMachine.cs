using UnityEngine;

namespace _01._Script.StataPattern
{
    public class StateMachine<T> where T : MonoBehaviour
    {
        public State<T> CurrentState { get; private set; }
        
        public void Initialize(State<T> startingState)
        {
            CurrentState = startingState;
            CurrentState.Enter();
        }

        public void ChangeState(State<T> newState)
        {
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }

        public void BoolInitialize(State<T> startingState)
        {
            CurrentState = startingState;
            CurrentState.BoolEnter();
        }

        public void BoolChangeState(State<T> newState)
        {
            CurrentState.BoolExit();
            CurrentState = newState;
            CurrentState.BoolEnter();
        }
    }
}