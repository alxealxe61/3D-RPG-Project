using UnityEngine;

namespace _01._Script
{
    /// <summary>
    /// 타격 판정이 발생하면 공격자와 피격자의 관계를 분석하고 적절한 데미지를 산출합니다.
    /// </summary>
    public class BattleSystem : MonoBehaviour
    {
        public static BattleSystem Instance { get; private set; }

        [Header("Layer Settings")]
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private LayerMask enemyLayer;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void ProcessHit(GameObject attacker, GameObject defender)
        {
            if (attacker == null || defender == null) return;

            // 1. 공격자의 데미지 계산
            int calculatedDamage = CalculateDamage(attacker);

            // 2. 피격자의 레이어에 따라 분기 처리
            if (((1 << defender.layer) & playerLayer.value) != 0)
            {
                HandlePlayerHit(defender, calculatedDamage);
            }
            else if (((1 << defender.layer) & enemyLayer.value) != 0)
            {
                HandleEnemyHit(attacker, defender, calculatedDamage);
            }
            else
            {
                HandleOtherHit(defender, calculatedDamage);
            }
        }

        private int CalculateDamage(GameObject attacker)
        {
            // [수정] 자식 오브젝트(HitBox)일 수 있으므로 부모에서 찾도록 변경
            PlayerStats stats = attacker.GetComponentInParent<PlayerStats>();
            if (stats != null)
            {
                return stats.CurrentAttack;
            }

            return 5; 
        }

        private void HandlePlayerHit(GameObject player, int damage)
        {
            if (player.TryGetComponent(out IDamageble damageable))
            {
                damageable.TakeDamage(damage);
                Debug.Log($"[Battle] <color=red>Player({player.name})</color> hit for {damage} damage.");
            }
        }

        private void HandleEnemyHit(GameObject attacker, GameObject enemy, int damage)
        {
            if (enemy.TryGetComponent(out IDamageble damageable))
            {
                damageable.TakeDamage(damage);
                Debug.Log($"[Battle] {enemy.name} 타격 성공! (데미지: {damage})");

                // --- [명중 포인트 지급 보완] ---
                // 공격자 또는 공격자의 부모에서 PlayerStats를 찾습니다.
                PlayerStats playerStats = attacker.GetComponentInParent<PlayerStats>();
                if (playerStats != null)
                {
                    playerStats.AddSkillPoint(0.5f);
                    Debug.Log($"[Battle] 스킬 포인트 0.5 지급 완료! 현재 포인트: {playerStats.CurrentSkillPoint}");
                }
                else
                {
                    Debug.LogWarning($"[Battle] 공격자({attacker.name})로부터 PlayerStats를 찾을 수 없어 포인트를 지급하지 못했습니다.");
                }
            }
            else
            {
                Debug.LogWarning($"[Battle] {enemy.name}에게 IDamageble이 없습니다.");
            }
        }

        private void HandleOtherHit(GameObject target, int damage)
        {
            if (target.TryGetComponent(out IDamageble damageable))
            {
                damageable.TakeDamage(damage);
            }
        }
    }
}
