using System;
using UnityEngine;

namespace _01._Script
{
    public class PlayerStats : MonoBehaviour, IDamageble
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
        public float CurrentSkillPoint { get; private set; } // float으로 변경
        public float MaxSkillPoint => playerProfile.maxSkillPoint;  // 최대 포인트
        private const float SkillUsageCost = 8f;             // 사용 소모량

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

        public void TakeDamage(int damage)
        {
            CurrentHp = Mathf.Max(CurrentHp - damage, 0);
            OnHpChanged?.Invoke(CurrentHp, (float)MaxHp);

            if (CurrentHp <= 0) Die();
        }

        // --- [스킬 포인트 관리] ---

        /// <summary>
        /// 지정된 양만큼 스킬 포인트를 추가합니다. (최대 20)
        /// </summary>
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
