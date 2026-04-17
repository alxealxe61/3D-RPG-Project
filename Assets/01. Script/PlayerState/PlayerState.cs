using _01._Script;
using _01._Script.StataPattern;
using UnityEngine;

public abstract class PlayerState : State<PlayerController>
{
    protected PlayerController player;
    protected PlayerStateMachine stateMachine;
    
    private int animHash;

    protected PlayerState(PlayerController player, 
        StateMachine<PlayerController> stateMachine, string animName) 
        : base(player, stateMachine, animName) { }

    public virtual void Enter()
    {
        if (animHash != 0)
        {
            player.ani.SetTrigger(animHash);
        }
    }

    public virtual void BoolEnter()
    {
        if (animHash != 0)
        {
            player.ani.SetBool(animHash, true);
        }
    }

    public virtual void BoolExit()
    {
        if (animHash != 0)
        {
            player.ani.SetBool(animHash, false);
        }
    }

    public virtual void Exit()
    {
        if (animHash != 0)
        {
            player.ani.ResetTrigger(animHash); 
        }
    }
    
    protected float GetNormalizedTime()
    {
        AnimatorStateInfo stateInfo = player.ani.GetCurrentAnimatorStateInfo(0);
        
        if (!player.ani.IsInTransition(0))
        {
            return stateInfo.normalizedTime;
        }
        return 0;
    }
    
    public virtual void LogicUpdate() { }
    public virtual void PhysicsUpdate() { }
}
