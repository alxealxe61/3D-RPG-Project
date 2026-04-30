using System;
using _01._Script.CombatSystem;
using _01._Script.Enemy.Range_Enemy;
using _01._Script.Item;
using UnityEngine;

namespace _01._Script.Data
{
    public class RangeStats : MonoBehaviour, ICombatAgent
    {
        [SerializeField] 
        private EnemyProfile rangeEnemyProfile;

        [SerializeField] 
        private RangeController rangeEnemyController;
        
        public int currentHp;
        public bool IsDead => currentHp <= 0;
        private int MaxHp => rangeEnemyProfile.MaxHp;
        private int CurrentAttack => rangeEnemyProfile.MaxAttack;
        public int MoveSpeed => rangeEnemyProfile.moveSpeed;
        
        public event Action<float, float> OnHpChanged;
        
        private void Awake() => currentHp = rangeEnemyProfile.MaxHp;

        private void Start()
        {
            var allDetector = GetComponentsInChildren<IHitDetector>(true);
            foreach (var detector in allDetector) detector.Initialize(this);
            
            var allHurtBox = GetComponentsInChildren<HurtBox>(true);
            foreach (var hurtBox in allHurtBox) hurtBox.Initialize(this);
        }
        // 데미지 받는 로직
        public void TakeDamage(int damage)
        {
            currentHp = Mathf.Max(currentHp - damage, 0);
            OnHpChanged?.Invoke(currentHp, MaxHp);
        }

        // 데미지 주는 로직
        public void OnHitDetected(HitInfo hitInfo)
        {
            var @event = new CombatEvent
            {
                Sender = this,
                Receiver = hitInfo.Receiver,
                Damage = CurrentAttack,
                HitInfo = hitInfo
            };

            CombatSystem.CombatSystem.Instance.AddCombatEvent(@event);
        }

        public void Stun()
        {
            rangeEnemyController.isStunned();
        }

        public void Pull()
        {
            throw new NotImplementedException();
        }

        [ContextMenu("Drop Item")]
        public void Die()
        { 
            LootManager.Instance.DropItems("Range", transform.position);
        }
    }
}