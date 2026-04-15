using UnityEngine;
using TMPro;
using UnityEngine.UI;
using _01._Script; // PlayerStats 네임스페이스

namespace _01._Script.UI_Manager
{
    public class UserInfoUI : MonoBehaviour
    {
        [Header("Player Reference")]
        [SerializeField] private PlayerStats playerStats;

        [Header("HP UI")]
        [SerializeField] private Slider hpSlider;
        [SerializeField] private TextMeshProUGUI hpText;

        [Header("Skill UI")]
        [SerializeField] private Slider skillSlider;
        [SerializeField] private TextMeshProUGUI skillText;

        [Header("Currency UI")]
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private TextMeshProUGUI upgradeStoneText;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            if (playerStats == null)
            {
                playerStats = FindFirstObjectByType<PlayerStats>();
            }

            if (playerStats != null)
            {
                SubscribeEvents();
                // 초기 UI 상태를 강제로 갱신 (Stats의 Start()에서 불릴 수도 있지만 여기서 한 번 더 안전하게 처리)
                UpdateHpUI(playerStats.CurrentHp, playerStats.MaxHp);
                UpdateSkillUI(playerStats.CurrentSkillPoint, 20f); // Max는 20 고정
                UpdateCurrencyUI();
            }
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            if (playerStats == null) return;

            playerStats.OnHpChanged += UpdateHpUI;
            playerStats.OnSkillPointChanged += UpdateSkillUI;
            playerStats.OnCurrencyChanged += UpdateCurrencyUI;
        }

        private void UnsubscribeEvents()
        {
            if (playerStats != null)
            {
                playerStats.OnHpChanged -= UpdateHpUI;
                playerStats.OnSkillPointChanged -= UpdateSkillUI;
                playerStats.OnCurrencyChanged -= UpdateCurrencyUI;
            }
        }

        private void UpdateHpUI(float currentHp, float maxHp)
        {
            if (hpSlider != null)
            {
                hpSlider.maxValue = maxHp;
                hpSlider.value = currentHp;
            }

            if (hpText != null)
            {
                hpText.text = $"{currentHp:N0} / {maxHp:N0}";
            }
        }

        private void UpdateSkillUI(float currentSkill, float maxSkill)
        {
            if (skillSlider != null)
            {
                skillSlider.maxValue = maxSkill;
                skillSlider.value = currentSkill;
            }

            if (skillText != null)
            {
                // 스킬이 사용 가능한지(8포인트 이상) 시각적으로 구분해주면 좋습니다.
                string availability = currentSkill >= 8 ? "<color=yellow>[OK]</color>" : "";
                skillText.text = $"{currentSkill:N0} / {maxSkill:N0} {availability}";
            }
        }

        public void UpdateCurrencyUI()
        {
            if (goldText != null)
            {
                goldText.text = playerStats.CurrentGold.ToString("N0");
            }

            if (upgradeStoneText != null)
            {
                upgradeStoneText.text = playerStats.CurrentUpgradeStones.ToString("N0");
            }
        }
    }
}
