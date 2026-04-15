using System.Collections.Generic;
using UnityEngine;

namespace _01._Script.Hit_Hurt_Box_Scripts
{
    public class HitBox : MonoBehaviour
    {
        [SerializeField]
        private GameObject owner;

        [SerializeField]
        private LayerMask targetLayerMask;

        public List<Collider> alreadyHitList = new List<Collider>();
        public Collider hitBoxCollider;

        private void Awake()
        {
            hitBoxCollider = GetComponent<Collider>();
            if (hitBoxCollider != null)
            {
                hitBoxCollider.isTrigger = true;
                hitBoxCollider.enabled = false;
            }
        }

        [ContextMenu("Start Attack")]
        public void StartAttack()
        {
            alreadyHitList.Clear();
            
            if (hitBoxCollider != null)
            {
                hitBoxCollider.enabled = true;
                CheckOverlapInstant();
            }
        }

        private void CheckOverlapInstant()
        {
            if (hitBoxCollider == null) return;

            Collider[] hitColliders = null;

            if (hitBoxCollider is BoxCollider box)
            {
                Vector3 center = transform.TransformPoint(box.center);
                Vector3 halfExtents = Vector3.Scale(box.size, transform.lossyScale) * 0.5f;
                hitColliders = Physics.OverlapBox(center, halfExtents, transform.rotation, targetLayerMask);
            }
            else if (hitBoxCollider is SphereCollider sphere)
            {
                Vector3 center = transform.TransformPoint(sphere.center);
                float radius = sphere.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
                hitColliders = Physics.OverlapSphere(center, radius, targetLayerMask);
            }

            if (hitColliders != null)
            {
                foreach (var col in hitColliders)
                {
                    ProcessHit(col);
                }
            }
        }

        [ContextMenu("Stop Attack")]
        public void StopAttack()
        {
            if (hitBoxCollider != null)
            {
                hitBoxCollider.enabled = false;
            }
            alreadyHitList.Clear(); 
        }

        private void OnTriggerEnter(Collider other)
        {
            ProcessHit(other);
        }

        private void ProcessHit(Collider other)
        {
            if (alreadyHitList.Contains(other)) return;

            if (((1 << other.gameObject.layer) & targetLayerMask.value) != 0)
            {
                if (other.TryGetComponent(out HurtBox hurtBox))
                {
                    alreadyHitList.Add(other);
                    // 데미지 없이 충돌 사실만 전달
                    hurtBox.OnHit(owner);
                }
            }
        }
    }
}
