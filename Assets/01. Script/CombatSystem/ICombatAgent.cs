using UnityEngine;

namespace _01._Script.CombatSystem
{
    public struct HitInfo
    {
        public ICombatAgent Receiver;
        public HurtBox HurtBox;
        public LayerMask LayerMask;
        public bool Stun;
        public bool Pull;
    }
    
    public interface ICombatAgent
    {
        void TakeDamage(int damage);
        void OnHitDetected(HitInfo hitInfo);
        void Stun();
        void Pull();
    }
}