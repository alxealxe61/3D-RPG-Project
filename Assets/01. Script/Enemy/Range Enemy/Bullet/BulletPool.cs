using System.Collections.Generic;
using UnityEngine;

namespace _01._Script.Enemy.Range_Enemy
{
    public class BulletPool : SingletonBase<BulletPool>
    {
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private int poolSize = 3;
        
        private readonly Queue<Bullet> poolQueue = new Queue<Bullet>();

        protected override void Awake()
        {
            base.Awake();
            for (int i = 0; i < poolSize; i++)
            {
                CreateNewBullet();
            }
        }

        private void CreateNewBullet()
        {
            Bullet bullet = Instantiate(bulletPrefab, transform);
            bullet.gameObject.SetActive(false);
            poolQueue.Enqueue(bullet);
        }

        public Bullet Get()
        {
            if (poolQueue.Count == 0) CreateNewBullet();
            
            Bullet bullet = poolQueue.Dequeue();
            bullet.gameObject.SetActive(true);
            return bullet;
        }

        public void ReturnToPool(Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
            poolQueue.Enqueue(bullet);
        }
    }
}