using UnityEngine;

namespace _01._Script.CombatSystem
{
    [RequireComponent(typeof(Collider))]
    public class HurtBox : MonoBehaviour, IHitTargetPart
    {
        public ICombatAgent Owner { get; set; }
    
        public Collider Collider { get; private set; }

        private void Awake()
        {
            Collider = GetComponent<Collider>();
        }
    
        public void Initialize(ICombatAgent owner)
        {
            Owner = owner;
            CombatSystem.Instance.AddHurtBox(Collider, this);
        }
    }
}