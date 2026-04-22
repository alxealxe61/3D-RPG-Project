using _01._Script;
using _01._Script.StataPattern;
using UnityEngine;

public abstract class PlayerState : State<PlayerController>
{
    protected PlayerController player => owner;
    
    private readonly int animHash;
    
    private readonly bool useBool;

    protected PlayerState(PlayerController player,
        StateMachine<PlayerController> stateMachine, string animName, bool useBool = false)
        : base(player, stateMachine, animName)
    {
        this.stateMachine = stateMachine;
        this.useBool = useBool;
        animHash = Animator.StringToHash(animName);
    }

    public override void Enter()
    {
        if (animHash == 0) return;
        if (useBool)
        {
            player.ani.SetBool(animHash, true);
        }
        else
        {
            player.ani.SetTrigger(animHash);
        }
    }
    
    public override void Exit()
    {
        if(animHash == 0) return;

        if (useBool)
        {
            player.ani.SetBool(animHash, false);
        }
        else
        {
            player.ani.ResetTrigger(animHash); 
        }
    }
    
    protected float GetNormalizedTime()
    {
        AnimatorStateInfo stateInfo = player.ani.GetCurrentAnimatorStateInfo(0);
        
        if (player.ani.IsInTransition(0) == false)
        {
            return stateInfo.normalizedTime;
        }
        return 0;
    }
}
