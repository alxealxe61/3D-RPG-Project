using System;
using _01._Script.CombatSystem;
using _01._Script.Enemy.Melee_Enemy;
using _01._Script.Item;
using UnityEngine;

namespace _01._Script.Data
{
    public class MeleeStats : MonoBehaviour, ICombatAgent
    {
        [SerializeField] 
        private EnemyProfile meleeEnemyProfile;
        
        [SerializeField] 
        private MeleeController meleeEnemyController;

        public int currentHp;
        public bool IsDead => currentHp <= 0;
        private int MaxHp => meleeEnemyProfile.MaxHp;
        private int CurrentAttack => meleeEnemyProfile.MaxAttack;
        public int MoveSpeed => meleeEnemyProfile.moveSpeed;
        private void Awake() => currentHp = meleeEnemyProfile.MaxHp;

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
        }

        // 데미지 주는 로직
        public void OnHitDetected(HitInfo hitInfo)
        {
            CombatEvent @event = new CombatEvent();
            @event.Sender = this;
            @event.Receiver = hitInfo.Receiver;
            @event.Damage = CurrentAttack;
            @event.HitInfo = hitInfo;
            
            CombatSystem.CombatSystem.Instance.AddCombatEvent(@event);
        }

        public void Stun()
        {
            meleeEnemyController.IsStunned();
        }

        public void Pull()
        {
            throw new NotImplementedException();
        }
        
        public void Die()
        {
            LootManager.Instance.DropItems("Melee", transform.position);
        }
    }
}