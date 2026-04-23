using System;
using _01._Script.CombatSystem;
using UnityEngine;

namespace _01._Script.Enemy.Range_Enemy.Range_Data
{
    public class RangeStats : MonoBehaviour, ICombatAgent
    {
        [SerializeField] 
        private EnemyProfile rangeEnemyProfile;

        [SerializeField] 
        private RangeController rangeEnemyController;
        
        public int currentHp;
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
                Receiver = hitInfo.receiver,
                Damage = CurrentAttack,
                HitInfo = hitInfo
            };

            CombatSystem.CombatSystem.Instance.AddCombatEvent(@event);
        }

        public void Stun()
        {
            rangeEnemyController.isStunned();
        }


        private void Die()
        {
            Debug.Log("Enemy Died");
        }
    }
}