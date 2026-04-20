using System;
using _01._Script.CombatSystem;
using UnityEngine;

namespace _01._Script
{
    public class PlayerStats : MonoBehaviour, ICombatAgent
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
            var allDetector = GetComponentsInChildren<IHitDetector>(true);
            foreach (var detector in allDetector) detector.Initialize(this);
            
            var allHurtBox = GetComponentsInChildren<HurtBox>(true);
            foreach (var hurtBox in allHurtBox) hurtBox.Initialize(this);
            OnHpChanged?.Invoke(CurrentHp, MaxHp);
            OnSkillPointChanged?.Invoke(CurrentSkillPoint, MaxSkillPoint);
            OnCurrencyChanged?.Invoke();
        }

        // ICombatAgent 구현: 피격 시 호출
        
        public void TakeDamage(int damage)
        {
            CurrentHp = Mathf.Max(CurrentHp - damage, 0);
            OnHpChanged?.Invoke(CurrentHp, (float)MaxHp);
        }

        // ICombatAgent 구현: 공격 성공 시 호출
        
        public void OnHitDetected(HitInfo hitInfo)
        {
            CombatEvent @event = new CombatEvent();
            @event.Sender = this;
            @event.Receiver = hitInfo.receiver;
            @event.Damage = CurrentAttack;
            @event.HitInfo = hitInfo;
            
            CombatSystem.CombatSystem.Instance.AddCombatEvent(@event);
        }
        

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
