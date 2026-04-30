using System.Collections.Generic;
using _01._Script.CombatSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace _01._Script.Enemy.Range_Enemy.Bullet
{
    public class Bullet : MonoBehaviour, IHitDetector
    {
        [SerializeField] private float speed = 15f;
        [SerializeField] private float lifeTime = 3f;

        [field: SerializeField] private Collider Collider { get; set; }

        [SerializeField] private BulletPool bulletPool;

        private Vector3 _moveDirection;
        private bool _isLaunched;

        private HashSet<ICombatAgent> hitAgents = new HashSet<ICombatAgent>();

        public ICombatAgent Owner { get; private set; }

        public void Initialize(ICombatAgent owner)
        {
            Owner = owner;
            Collider = GetComponent<Collider>();
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

        public void Launch(Vector3 targetPos)
        {
            var spawnPos = transform.position;
            targetPos.y = spawnPos.y - 1;

            _moveDirection = (targetPos - spawnPos).normalized;
            transform.rotation = Quaternion.LookRotation(_moveDirection);
            _isLaunched = true;
            EnableDetection();
            Invoke(nameof(ReturnToPool), lifeTime);
        }

        private void Update()
        {
            if (_isLaunched == false) return;
            transform.position += _moveDirection * speed * Time.deltaTime;
        }

        private void ReturnToPool()
        {
            DisableDetection();
            _isLaunched = false;
            CancelInvoke();
            bulletPool.ReturnToPool(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (CombatSystem.CombatSystem.Instance == null || CombatSystem.CombatSystem.Instance.HasHurtBox(other) == false) return;
            if (other.gameObject.layer == this.gameObject.layer) return;
        
            var hurtBox = CombatSystem.CombatSystem.Instance.GetHurtBox(other);
            if (hurtBox == null || hurtBox.Owner == null) return;
        
            var receiver = hurtBox.Owner;
            if (hitAgents.Contains(receiver)) return;
            hitAgents.Add(receiver);

            var hitInfo = new HitInfo();
            hitInfo.HurtBox = hurtBox;
            hitInfo.Receiver = receiver;
        
            ReturnToPool();

            Owner?.OnHitDetected(hitInfo);
        }
    }
}