using System;
using UnityEngine;

namespace _01._Script.Enemy_Data
{
    public class EnemyStats : MonoBehaviour
    {
        [SerializeField] 
        private EnemyProfile enemyProfile;
        
        public int CurrentHp { get; private set; }
        public int CurrentAttack => enemyProfile.MaxAttack;
        public int moveSpeed => enemyProfile.moveSpeed;
        
        public event Action<float, float> OnHpChanged;
        
        private void Awake() => CurrentHp = enemyProfile.MaxHp;
        
        public void TakeDamage(int damage)
        {
            CurrentHp = Mathf.Max(CurrentHp - damage, 0);
            OnHpChanged?.Invoke(CurrentHp, (float)enemyProfile.MaxHp);

            Debug.Log($"[Enemy] {damage} 데미지 발생! 현재 체력: {CurrentHp}");
            if (CurrentHp <= 0) Die();
        }

     

        private void Die()
        {
            Debug.Log("Enemy Died");
        }
    }
}