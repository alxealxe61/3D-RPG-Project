using UnityEngine;

namespace _01._Script.Enemy.Range_Enemy
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed = 15f;
        [SerializeField] private float lifeTime = 3f;
        
        private Vector3 moveDirection;
        private bool isLaunched;

        public void Launch(Vector3 targetPos)
        {
            Vector3 spawnPos = transform.position;
            targetPos.y = spawnPos.y - 1;
            
            moveDirection = (targetPos - spawnPos).normalized;
            isLaunched = true;
            
            Invoke(nameof(ReturnToPool), lifeTime);
        }

        void Update()
        {
            if (isLaunched == false) return;
            transform.position += moveDirection * speed * Time.deltaTime;  
        }

        private void ReturnToPool()
        {
            Debug.Log("Returning to the pool");
            isLaunched = false;
            CancelInvoke();
            BulletPool.Instance.ReturnToPool(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                ReturnToPool();
            }
        }
    }
}