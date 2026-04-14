using _01._Script;
using UnityEngine;

public abstract class PlayerState
{
    protected PlayerController player;
    protected PlayerStateMachine stateMachine;
    
    protected int animHash;

    protected PlayerState(PlayerController player, PlayerStateMachine stateMachine, string animName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animHash = Animator.StringToHash(animName);
    }

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
    public virtual void HandleInput() { }
    public virtual void OnCollisionEnter(Collision collision) { }
    public virtual void OnTriggerEnter(Collider other) { }
    
}
