using System;
using UnityEngine;

namespace _01._Script
{
    public class PlayerStats : MonoBehaviour, IDamageble
    {
        [SerializeField] 
        private PlayerProfile playerProfile;
        
        // 플레이어 스탯
        public int CurrentHp { get; private set; }
        public int CurrentAttack => playerProfile.MaxAttack;
        public int moveSpeed => playerProfile.moveSpeed;

        public int CurrentGold => playerProfile.gold;
        public int CurrentUpgradeStones => playerProfile.upgradeStones;
        
        public event Action<float, float> OnHpChanged;
        
        private void Awake() => CurrentHp = playerProfile.MaxHp;
        
        public void TakeDamage(int damage)
        {
            damage = CurrentAttack;
            CurrentHp = Mathf.Max(CurrentHp - damage, 0);
            
            OnHpChanged?.Invoke(CurrentHp, playerProfile.MaxHp);

            if (CurrentHp <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log("Player Died");
        }
    }
}