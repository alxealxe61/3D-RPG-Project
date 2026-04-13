using _01._Script;
using UnityEngine;

public abstract class PlayerState
{
    protected PlayerController player;
    protected PlayerStateMachine stateMachine;
    
    protected float startTime;
    
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

    public virtual void Exit()
    {
        if (animHash != 0)
        {
            player.ani.ResetTrigger(animHash); 
        }
    }
    
    public virtual void LogicUpdate() { }
    public virtual void PhysicsUpdate() { }
    public virtual void HandleInput() { }
    public virtual void OnCollisionEnter(Collision collision) { }
    public virtual void OnTriggerEnter(Collider other) { }
    
}
