using System.Collections.Generic;
using UnityEngine;

namespace _01._Script.Enemy.Range_Enemy.Bullet
{
    public class BulletPool : MonoBehaviour
    {
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private int poolSize = 3;
        
        private readonly Queue<Bullet> _poolQueue = new Queue<Bullet>();

        private void Awake()
        {
            for (int i = 0; i < poolSize; i++)
            {
                CreateNewBullet();
            }
        }

        private void CreateNewBullet()
        {
            var bullet = Instantiate(bulletPrefab, transform);
            bullet.gameObject.SetActive(false);
            _poolQueue.Enqueue(bullet);
        }

        public Bullet Get()
        {
            if (_poolQueue.Count == 0) CreateNewBullet();
            
            var bullet = _poolQueue.Dequeue();
            bullet.gameObject.SetActive(true);
            return bullet;
        }

        public void ReturnToPool(Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
            _poolQueue.Enqueue(bullet);
        }
    }
}