using System;
using _01._Script.CombatSystem;
using UnityEngine;

namespace _01._Script.Enemy_Data
{
    public class EnemyStats : MonoBehaviour, ICombatAgent
    {
        [SerializeField] 
        private EnemyProfile enemyProfile;
        
        public int CurrentHp { get; private set; }
        public int CurrentAttack => enemyProfile.MaxAttack;
        public int moveSpeed => enemyProfile.moveSpeed;
        
        public event Action<float, float> OnHpChanged;
        
        private void Awake() => CurrentHp = enemyProfile.MaxHp;

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
            //Debug.Log($"{gameObject.name}: TakeDamage {damage}");
        }

        // 데미지 주는 로직
        public void OnHitDetected(HitInfo hitInfo)
        {
            CombatEvent @event = new CombatEvent();
            @event.Sender = this;
            @event.Receiver = hitInfo.receiver;
            @event.Damage = CurrentAttack;
            @event.HitInfo = hitInfo;
            
            CombatSystem.CombatSystem.Instance.AddCombatEvent(@event);
        }


        private void Die()
        {
            Debug.Log("Enemy Died");
        }
    }
}