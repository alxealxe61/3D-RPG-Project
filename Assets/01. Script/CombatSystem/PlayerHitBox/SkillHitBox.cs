using System.Collections.Generic;
using UnityEngine;

namespace _01._Script.CombatSystem.PlayerHitBox
{
    public class SkillHitBox : MonoBehaviour, IHitDetector
    {
        public ICombatAgent Owner { get; private set; }
    
        [field: SerializeField]  private Collider Collider { get; set; }
    
        private HashSet<ICombatAgent> hitAgents = new HashSet<ICombatAgent>();
    
        public void Initialize(ICombatAgent owner)
        {
            Owner = owner;
            Collider =  GetComponent<Collider>();
        }

        [ContextMenu("Start Attack")]
        public void EnableDetection()
        {
            Collider.enabled = true;
        }

        [ContextMenu("Stop Attack")]
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
            hitInfo.Stun = true;
        
            Owner.OnHitDetected(hitInfo);
        }
    }
}