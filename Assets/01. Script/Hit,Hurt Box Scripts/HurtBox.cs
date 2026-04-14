using UnityEngine;

namespace _01._Script.Hit_Hurt_Box_Scripts
{
    public class HurtBox : MonoBehaviour
    {
        [SerializeField]
        private GameObject owner;
        
        private IDamageble damageable;

        private void Awake()
        {
            damageable = owner.GetComponent<IDamageble>();
        }

        public void OnHit(int damage)
        {
            if (damage != null)
            {
                damageable.TakeDamage(damage);
            }
        }
    }
}