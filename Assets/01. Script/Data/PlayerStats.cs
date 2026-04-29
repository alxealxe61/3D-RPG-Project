using System;
using _01._Script.CombatSystem;
using _01._Script.Data;
using _01._Script.Item;
using UnityEngine;

namespace _01._Script
{
    public class PlayerStats : MonoBehaviour, ICombatAgent
    {
        [SerializeField] 
        private DataProfile playerProfile;
        
        [SerializeField] 
        private PlayerController playerController;
        
        private const float PERFECT_GUARD_WINDOW = 1.0f;
        private const float GUARD_DAMAGE_REDUCTION = 0.3f;
        
        // --- [기본 스탯] ---
        public int CurrentHp { get; private set; }
        public int MaxHp => playerProfile.MaxHp;
        private int CurrentAttack => playerProfile.MaxAttack;
        public int MoveSpeed => playerProfile.moveSpeed;

        // --- [재화] ---
        public int CurrentGold => playerProfile.gold;
        public int CurrentUpgradeStones => playerProfile.upgradeStones;

        // --- [스킬 포인트] ---
        public float currentSkillPoint = 8;
        private float MaxSkillPoint => playerProfile.maxSkillPoint;  
        private const float SkillUsageCost = 8f;             

        // --- [이벤트] ---
        public event Action<float, float> OnHpChanged;
        public event Action<float, float> OnSkillPointChanged; 
        public event Action OnCurrencyChanged;                

        private void Awake()
        {
            // DataManager에서 활성화된 프로필을 가져옵니다. (불러오기 창에서 로드된 데이터)
            playerProfile = DataManager.Instance.ActiveProfile;
            
            CurrentHp = MaxHp;
        }
        
        private void Start()
        {
            var allDetector = GetComponentsInChildren<IHitDetector>(true);
            foreach (var detector in allDetector) detector.Initialize(this);
            
            var allHurtBox = GetComponentsInChildren<HurtBox>(true);
            foreach (var hurtBox in allHurtBox) hurtBox.Initialize(this);
            OnHpChanged?.Invoke(CurrentHp, MaxHp);
            OnSkillPointChanged?.Invoke(currentSkillPoint, MaxSkillPoint);
            OnCurrencyChanged?.Invoke();
        }

        public void AddItem(ItemType type, int count)
        {
            switch (type)
            {
                case ItemType.Gold:
                    playerProfile.gold += count;
                    break;
                case ItemType.EnhancementStone:
                    playerProfile.upgradeStones += count;
                    break;
                case ItemType.QuestItem:
                    break;
            }
            
            OnCurrencyChanged?.Invoke();
        }
        
        // ICombatAgent 구현: 피격 시 호출
        
        public void TakeDamage(int damage)
        {
            if (playerController.IsGuarding == true)
            {
                if (playerController.GuardTimer <= PERFECT_GUARD_WINDOW)
                {
                    EffectManager.Instance.PlayEffect("PerfectGuardEffect", transform);
                    SoundManager.Instance.PlaySFX("PerfectGuardSound", transform.position);
                    return;
                }

                int reducedDamage = Mathf.RoundToInt(damage * GUARD_DAMAGE_REDUCTION);
                SoundManager.Instance.PlaySFX("GuardSound", transform.position);
                CurrentHp = Mathf.Max(CurrentHp - reducedDamage, 0);
            }
            else
            {
                CurrentHp = Mathf.Max(CurrentHp - damage, 0);
            }
            
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

        public void Stun()
        {
            if (playerController.IsGuarding == true) return;
            playerController.isStunned();
        }

        public void Pull() => playerController.isPulling();
        
        public void AddSkillPoint(float amount)
        {
            if (currentSkillPoint < MaxSkillPoint)
            {
                currentSkillPoint = Mathf.Min(currentSkillPoint + amount, MaxSkillPoint);
                OnSkillPointChanged?.Invoke(currentSkillPoint, MaxSkillPoint);
            }
        }
        
        public bool CanUseSkill() => currentSkillPoint >= SkillUsageCost;

        public void UseSkill()
        {
            if (CanUseSkill())
            {
                currentSkillPoint -= SkillUsageCost;
                OnSkillPointChanged?.Invoke(currentSkillPoint, MaxSkillPoint);
            }
        }

        public void FullRecover()
        {
            CurrentHp = MaxHp;
            currentSkillPoint = MaxSkillPoint;
            OnHpChanged?.Invoke(CurrentHp, (float)MaxHp);
            OnSkillPointChanged?.Invoke(currentSkillPoint, MaxSkillPoint);
        }
    }
}
