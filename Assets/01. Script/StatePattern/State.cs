using UnityEngine;

namespace _01._Script.StatePattern
{
    public abstract class State<T> where T : MonoBehaviour
    {
        protected readonly T Owner;
        protected StateMachine<T> StateMachine;
        protected string AniName;

        protected State(T owner, StateMachine<T> stateMachine, string aniName)
        {
            Owner = owner;
            StateMachine = stateMachine;
            AniName = aniName;
        }

        protected internal virtual void Enter() { }
        protected internal virtual void Exit() { }
        protected internal virtual void LogicUpdate() { }
        protected internal virtual void PhysicsUpdate() { }
    }
}