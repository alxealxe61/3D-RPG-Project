using UnityEngine;

namespace _01._Script.CombatSystem
{
    [RequireComponent(typeof(Collider))]
    public class HurtBox : MonoBehaviour
    {
        public ICombatAgent Owner { get; private set; }

        private Collider Collider { get; set; }

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