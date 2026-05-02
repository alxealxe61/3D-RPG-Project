using _01._Script.Data;
using _01._Script.Enemy.Boss_Enemy;
using UnityEngine;
using UnityEngine.UI;

namespace _01._Script.UI
{
    public class BossHpUi : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private BossController bossController; // 보스 컨트롤러 참조
        [SerializeField] private GameObject hpBarContainer;     // 체력바 부모 오브젝트 (Show/Hide용)
        [SerializeField] private Slider hpSlider;               // UI 슬라이더

        private BossStats _bossStats;

        private void Start()
        {
            if (bossController != null)
            {
                _bossStats = bossController.bossStats;
                
                // HP 변경 이벤트 구독
                if (_bossStats != null)
                {
                    _bossStats.OnHpChanged += UpdateHpBar;
                    
                    // 초기 체력 설정
                    UpdateHpBar(_bossStats.currentHp, _bossStats.MaxHp);
                }
            }
            
            // 시작 시 UI 숨김
            if (hpBarContainer != null)
                hpBarContainer.SetActive(false);
        }

        private void OnDestroy()
        {
            // 이벤트 구독 해제 (메모리 누수 방지)
            if (_bossStats != null)
            {
                _bossStats.OnHpChanged -= UpdateHpBar;
            }
        }

        private void Update()
        {
            if (bossController == null || hpBarContainer == null) return;

            // 보스가 플레이어를 감지했는지 여부에 따라 UI 활성화/비활성화
            bool shouldShow = bossController.Target != null && _bossStats.IsDead == false;
            
            if (hpBarContainer.activeSelf != shouldShow)
            {
                hpBarContainer.SetActive(shouldShow);
            }
            
        }

        private void UpdateHpBar(float currentHp, float maxHp)
        {
            if (hpSlider != null && maxHp > 0)
            {
                // 실시간 체력 비율 반영
                hpSlider.value = currentHp / maxHp;
            }
        }
    }
}
