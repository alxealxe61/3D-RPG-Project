using _01._Script.StataPattern;
using UnityEngine;

namespace _01._Script.Player.PlayerState
{
    public abstract class PlayerState : State<PlayerController>
    {
        protected PlayerController Player => owner;
    
        private readonly int _animHash;
    
        private readonly bool _useBool;

        protected PlayerState(PlayerController player,
            StateMachine<PlayerController> stateMachine, string animName, bool useBool = false)
            : base(player, stateMachine, animName)
        {
            this.stateMachine = stateMachine;
            _useBool = useBool;
            _animHash = Animator.StringToHash(animName);
        }

        public override void Enter()
        {
            if (_animHash == 0) return;
            if (_useBool)
            {
                Player.ani.SetBool(_animHash, true);
            }
            else
            {
                Player.ani.SetTrigger(_animHash);
            }
        }
    
        public override void Exit()
        {
            if(_animHash == 0) return;

            if (_useBool)
            {
                Player.ani.SetBool(_animHash, false);
            }
            else
            {
                Player.ani.ResetTrigger(_animHash); 
            }
        }
    
        protected float GetNormalizedTime()
        {
            var stateInfo = Player.ani.GetCurrentAnimatorStateInfo(0);
        
            if (Player.ani.IsInTransition(0) == false)
            {
                return stateInfo.normalizedTime;
            }
            return 0;
        }
    }
    public class PlayerStateMachine : StateMachine<PlayerController> { }
}