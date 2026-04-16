using System;
using UnityEngine;

namespace _01._Script
{
    public class PlayerStats : MonoBehaviour
    {
        [SerializeField] 
        private PlayerProfile playerProfile;
        
        // --- [기본 스탯] ---
        public int CurrentHp { get; private set; }
        public int MaxHp => playerProfile.MaxHp;
        public int CurrentAttack => playerProfile.MaxAttack;
        public int moveSpeed => playerProfile.moveSpeed;

        // --- [재화] ---
        public int CurrentGold => playerProfile.gold;
        public int CurrentUpgradeStones => playerProfile.upgradeStones;

        // --- [스킬 포인트] ---
        public float CurrentSkillPoint { get; private set; } 
        public float MaxSkillPoint => playerProfile.maxSkillPoint;  
        private const float SkillUsageCost = 8f;             

        // --- [이벤트] ---
        public event Action<float, float> OnHpChanged;
        public event Action<float, float> OnSkillPointChanged; 
        public event Action OnCurrencyChanged;                

        private void Awake() => CurrentHp = MaxHp;
        
        private void Start()
        {
            OnHpChanged?.Invoke(CurrentHp, MaxHp);
            OnSkillPointChanged?.Invoke(CurrentSkillPoint, MaxSkillPoint);
            OnCurrencyChanged?.Invoke();
        }

        // ICombatAgent 구현: 피격 시 호출
        public void TakeDamage(int damage)
        {
            CurrentHp = Mathf.Max(CurrentHp - damage, 0);
            OnHpChanged?.Invoke(CurrentHp, (float)MaxHp);

            Debug.Log($"[Player] {damage} 데미지 발생! 현재 체력: {CurrentHp}");
            if (CurrentHp <= 0) Die();
        }

        // ICombatAgent 구현: 공격 성공 시 호출
        

        public void AddSkillPoint(float amount)
        {
            if (CurrentSkillPoint < MaxSkillPoint)
            {
                CurrentSkillPoint = Mathf.Min(CurrentSkillPoint + amount, MaxSkillPoint);
                OnSkillPointChanged?.Invoke(CurrentSkillPoint, MaxSkillPoint);
            }
        }
        
        public bool CanUseSkill() => CurrentSkillPoint >= SkillUsageCost;

        public void UseSkill()
        {
            if (CanUseSkill())
            {
                CurrentSkillPoint -= SkillUsageCost;
                OnSkillPointChanged?.Invoke(CurrentSkillPoint, MaxSkillPoint);
            }
        }

        private void Die() => Debug.Log("Player Died");
    }
}
