using System;
using _01._Script.CombatSystem;
using _01._Script.Enemy.Melee_Enemy;
using UnityEngine;

namespace _01._Script.Enemy_Data
{
    public class MeleeEnemyStats : MonoBehaviour, ICombatAgent
    {
        [SerializeField] 
        private EnemyProfile meleeEnemyProfile;
        
        [SerializeField] 
        private MeleeController meleeEnemyController;

        public int currentHp;
        private int MaxHp => meleeEnemyProfile.MaxHp;
        private int CurrentAttack => meleeEnemyProfile.MaxAttack;
        public int MoveSpeed => meleeEnemyProfile.moveSpeed;
        
        public event Action<float, float> OnHpChanged;
        
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
            OnHpChanged?.Invoke(currentHp, (float)MaxHp);
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

        public void Stun()
        {
            meleeEnemyController.isStunned();
        }


        private void Die()
        {
            Debug.Log("Enemy Died");
        }
    }
}