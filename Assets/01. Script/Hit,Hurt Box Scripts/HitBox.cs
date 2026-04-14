using System.Collections.Generic;
using UnityEngine;

namespace _01._Script.Hit_Hurt_Box_Scripts
{
    public class HitBox : MonoBehaviour
    {
        [SerializeField] private float damage = 10f;
        
        [SerializeField] private LayerMask targetLayerMask;
        
        private readonly List<Collider> alreadyHitList = new List<Collider>();

        private Collider hitBoxCollider;
        
        private void Awake()
        {
            hitBoxCollider = GetComponent<Collider>();
        }

        public void StartAttack()
        {
            alreadyHitList.Clear();
            hitBoxCollider.enabled = true;
        }

        public void StopAttack()
        {
            hitBoxCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (alreadyHitList.Contains(other) == true)
            {
                return;
            }

            if (((1 << other.gameObject.layer) & targetLayerMask.value) != 0)
            {
                if (other.TryGetComponent(out HitBox otherHitBox) == true)
                {
                    alreadyHitList.Add(other);
                    
                }
            }
        }
    }
}