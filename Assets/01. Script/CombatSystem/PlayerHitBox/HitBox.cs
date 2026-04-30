using System.Collections.Generic;
using UnityEngine;

namespace _01._Script.CombatSystem.PlayerHitBox
{
    public class HitBox : MonoBehaviour, IHitDetector
    {
        public ICombatAgent Owner { get; private set; }
    
        [field: SerializeField]  private Collider Collider { get; set; }
    
        private HashSet<ICombatAgent> hitAgents = new HashSet<ICombatAgent>();
    
        public void Initialize(ICombatAgent owner)
        {
            Owner = owner;
            Collider =  GetComponent<Collider>();
        }
    
        public void EnableDetection()
        {
            Collider.enabled = true;
        }
    
        public void DisableDetection()
        {
            Collider.enabled = false;
            hitAgents.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (CombatSystem.Instance.HasHurtBox(other) == false) return;
        
            var hurtBox = CombatSystem.Instance.GetHurtBox(other);
            var receiver = hurtBox.Owner;
            if (hitAgents.Contains(receiver)) return;
            hitAgents.Add(receiver);
        
            var hitInfo = new HitInfo();
            hitInfo.HurtBox = CombatSystem.Instance.GetHurtBox(other);
            hitInfo.Receiver = hitInfo.HurtBox.Owner;
        
            Owner.OnHitDetected(hitInfo);
        }
    }
}
