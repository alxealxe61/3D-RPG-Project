using System;
using _01._Script.CombatSystem;
using _01._Script.Enemy.Boss_Enemy;
using _01._Script.Item;
using UnityEngine;

namespace _01._Script.Data
{
    public class BossStats : MonoBehaviour,ICombatAgent
    {
        [SerializeField] 
        private EnemyProfile bossProfile;

        [SerializeField] 
        private BossController bossController;
        
        public int currentHp;
        public bool IsDead => currentHp <= 0;
        public int MaxHp => bossProfile.MaxHp;
        private int CurrentAttack => bossProfile.MaxAttack;
        public int MoveSpeed => bossProfile.moveSpeed;
        
        public event Action<float, float> OnHpChanged;
        
        private void Awake() => currentHp = bossProfile.MaxHp;

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
            bossController.isStunned();
        }

        public void Pull()
        {
            throw new NotImplementedException();
        }


        public void Die()
        {
            LootManager.Instance.DropItems("Boss", transform.position);
        }
    }
}