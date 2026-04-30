using System;
using _01._Script.CombatSystem;
using _01._Script.Effect;
using _01._Script.Item;
using _01._Script.Player;
using UnityEngine;

namespace _01._Script.Data
{
    public class PlayerStats : MonoBehaviour, ICombatAgent
    {
        [SerializeField] 
        private DataProfile playerProfile;
        
        [SerializeField] 
        private PlayerController playerController;
        
        private const float PerfectGuardWindow = 1.0f;
        private const float GuardDamageReduction = 0.3f;
        
        // --- [기본 스탯] ---
        public int CurrentHp { get; private set; }
        public int MaxHp => playerProfile.MaxHp;
        private int CurrentAttack => playerProfile.MaxAttack;
        public int MoveSpeed => playerProfile.moveSpeed;

        // --- [재화] ---
        public int CurrentGold => playerProfile.gold;
        public int CurrentUpgradeStones => playerProfile.upgradeStones;
        public int CurrentWeaponLevel => playerProfile.weaponLevel;

        // --- [스킬 포인트] ---
        public float currentSkillPoint = 8;
        private float MaxSkillPoint => playerProfile.maxSkillPoint;  
        private const float SkillUsageCost = 8f;             

        // --- [이벤트] ---
        public event Action<float, float> OnHpChanged;
        public event Action<float, float> OnSkillPointChanged; 
        public event Action OnCurrencyChanged;                
        public event Action<int> OnUpgradeChanged;

        private void Awake()
        {
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
            OnUpgradeChanged?.Invoke(CurrentWeaponLevel);
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

        public bool SpendResources(int gold, int stones)
        {
            if (playerProfile.gold < gold || playerProfile.upgradeStones < stones)
            {
                return false;
            }

            playerProfile.gold -= gold;
            playerProfile.upgradeStones -= stones;
            OnCurrencyChanged?.Invoke();
            return true;
        }

        public void UpgradeWeapon(int attackIncrease)
        {
            playerProfile.weaponLevel++;
            playerProfile.MaxAttack += attackIncrease;
            OnUpgradeChanged?.Invoke(playerProfile.weaponLevel);
        }
        
        public void MaxWeapon(int attackIncrease)
        {
            playerProfile.MaxAttack += attackIncrease;
        }
        
        // ICombatAgent 구현: 피격 시 호출
        
        public void TakeDamage(int damage)
        {
            if (CurrentHp <= 0) return;
            if (playerController.IsGuarding)
            {
                if (playerController.GuardTimer <= PerfectGuardWindow)
                {
                    EffectManager.Instance.PlayEffect("PerfectGuardEffect", transform);
                    SoundManager.Instance.PlaySFX("PerfectGuardSound", transform.position);
                    return;
                }

                int reducedDamage = Mathf.RoundToInt(damage * GuardDamageReduction);
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
            var @event = new CombatEvent();
            @event.Sender = this;
            @event.Receiver = hitInfo.Receiver;
            @event.Damage = CurrentAttack;
            @event.HitInfo = hitInfo;
            
            CombatSystem.CombatSystem.Instance.AddCombatEvent(@event);
        }

        public void Stun()
        {
            if (playerController.IsGuarding) return;
            playerController.IsStunned();
        }

        public void Pull() => playerController.IsPulling();
        
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
