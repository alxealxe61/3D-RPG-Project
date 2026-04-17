using System.Buffers;
using UnityEngine;

namespace _01._Script.StataPattern
{
    public abstract class State<T> where T : MonoBehaviour
    {
        protected readonly T owner;
        protected StateMachine<T> stateMachine;
        protected string aniName;

        protected State(T owner, StateMachine<T> stateMachine, string aniName)
        {
            this.owner = owner;
            this.stateMachine = stateMachine;
            //this.animName = animName;
            this.aniName = aniName;
        }

        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void BoolEnter() { }
        public virtual void BoolExit() { }
        public virtual void LogicUpdate() { }
        public virtual void PhysicsUpdate() { }
    }
}