using UnityEngine;

namespace _01._Script.Hit_Hurt_Box_Scripts
{
    /// <summary>
    /// 타격을 받는 판정 영역을 정의합니다.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class HurtBox : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("이 판정의 주인 오브젝트 (예: Player, Enemy 루트)")]
        private GameObject owner;

        private Collider hurtBoxCollider;

        private void Awake()
        {
            hurtBoxCollider = GetComponent<Collider>();
            if (hurtBoxCollider != null)
            {
                hurtBoxCollider.isTrigger = true;
            }

            if (owner == null)
            {
                owner = transform.root.gameObject;
            }
        }

        /// <summary>
        /// 타격 발생 시 HitBox에 의해 호출됩니다. 데미지 정보는 더 이상 여기서 처리하지 않습니다.
        /// </summary>
        public void OnHit(GameObject attacker)
        {
            if (BattleSystem.Instance != null)
            {
                // 데미지 인자 없이 attacker 정보만 넘깁니다.
                BattleSystem.Instance.ProcessHit(attacker, owner);
            }
        }

        public void SetHurtBoxActive(bool isActive)
        {
            if (hurtBoxCollider != null)
            {
                hurtBoxCollider.enabled = isActive;
            }
        }
    }
}
