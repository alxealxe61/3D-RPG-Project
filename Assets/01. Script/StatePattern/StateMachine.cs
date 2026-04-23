using UnityEngine;

namespace _01._Script.StataPattern
{
    public abstract class StateMachine<T> where T : MonoBehaviour
    {
        public State<T> CurrentState { get; private set; }

        public virtual void Initialize(State<T> startingState)
        {
            //if(CurrentState == startingState) return;
            CurrentState = startingState;
            CurrentState.Enter();
        }

        public virtual void ChangeState(State<T> newState)
        {
            if(CurrentState == newState) return;
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}