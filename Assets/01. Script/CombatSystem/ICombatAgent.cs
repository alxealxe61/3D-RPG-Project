using UnityEngine;

public struct HitInfo
{
    public ICombatAgent receiver;
    public HurtBox hurtBox;
    //public LayerMask layerMask;
    public bool stun;
    public bool pull;
}
    
public interface ICombatAgent
{
    void TakeDamage(int damage);
    void OnHitDetected(HitInfo hitInfo);
    void Stun();
    void Pull();
}