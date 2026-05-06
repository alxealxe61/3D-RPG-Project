using UnityEngine;

namespace _01._Script.StatePattern
{
    public abstract class StateMachine<T> where T : MonoBehaviour
    {
        public State<T> CurrentState { get; private set; }

        protected internal virtual void Initialize(State<T> startingState)
        {
            CurrentState = startingState;
            CurrentState.Enter();
        }

        protected internal virtual void ChangeState(State<T> newState)
        {
            if(CurrentState == newState) return;
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}