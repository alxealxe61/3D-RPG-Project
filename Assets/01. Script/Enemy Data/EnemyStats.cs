using System;
using UnityEngine;

namespace _01._Script.Enemy_Data
{
    public class EnemyStats : MonoBehaviour, IDamageble
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
            // 전달받은 데미지 수치만큼 체력을 깎도록 수정
            CurrentHp = Mathf.Max(CurrentHp - damage, 0);
            
            OnHpChanged?.Invoke(CurrentHp, (float)enemyProfile.MaxHp);

            if (CurrentHp <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log("Enemy Died");
        }
    }
}