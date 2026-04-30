using System.Collections;
using _01._Script.Data;
using _01._Script.Player;
using CrusaderUI.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _01._Script.UI
{
    public class UserInfoUI : MonoBehaviour
    {
        [Header("Player Reference")]
        [SerializeField] private PlayerStats playerStats;
        [SerializeField] private PlayerController playerController;
        
        [Header("HP UI")]
        [SerializeField] private HPFlowController hpFlowController;

        [Header("Skill UI")]
        [SerializeField] private HPFlowController SkillFlowController;
        [SerializeField] private GameObject SkillImage;

        [Header("Currency UI")]
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private TextMeshProUGUI upgradeStoneText;
        [SerializeField] private TextMeshProUGUI weaponLevelText;

        [Header("Dodge UI")]
        [SerializeField] private TextMeshProUGUI dodgeText;
        [SerializeField] private Image dodgeImage;

        void Awake()
        {
            // 씬 전환 시에도 유지되도록 설정 (마을 -> 보스 맵 등)
            DontDestroyOnLoad(gameObject);
        }

        private IEnumerator Start()
        {
            // 1. PlayerStats를 찾을 때까지 대기
            while (playerStats == null)
            {
                playerStats = FindFirstObjectByType<PlayerStats>();
                if (playerStats == null) yield return null;
            }

            // 2. 이벤트를 구독합니다.
            SubscribeEvents();
            
            // 3. [중요] 모든 객체의 Start()가 끝날 때까지 한 프레임 대기하여 데이터 로드를 보장합니다.
            yield return new WaitForEndOfFrame();
            
            // 4. 초기 UI 강제 갱신
            RefreshAllUI();
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
            playerStats.OnUpgradeChanged += UpdateUpgradeUI;

            if (playerStats.TryGetComponent<PlayerController>(out var controller))
            {
                playerController = controller;
                controller.OnDodgeCooldownChanged += UpdateDodgeUI;
            }
        }

        private void UnsubscribeEvents()
        {
            if (playerStats != null)
            {
                playerStats.OnHpChanged -= UpdateHpUI;
                playerStats.OnSkillPointChanged -= UpdateSkillUI;
                playerStats.OnCurrencyChanged -= UpdateCurrencyUI;
                playerStats.OnUpgradeChanged -= UpdateUpgradeUI;

                if (playerController != null)
                {
                    playerController.OnDodgeCooldownChanged -= UpdateDodgeUI;
                }
            }
        }

        public void RefreshAllUI()
        {
            if (playerStats == null) return;

            UpdateHpUI(playerStats.CurrentHp, playerStats.MaxHp);
            UpdateSkillUI(playerStats.currentSkillPoint, 20f); // Max 스킬 포인트는 20 고정
            UpdateCurrencyUI();
            UpdateUpgradeUI(playerStats.CurrentWeaponLevel);
            
            if (playerController != null)
            {
                UpdateDodgeUI(playerController.dodgetime, PlayerController.DodgeCooldown);
            }
        }

        private void UpdateHpUI(float currentHp, float maxHp)
        {
            if (hpFlowController != null && maxHp > 0)
            {
                hpFlowController.SetValue(currentHp / maxHp);
            }
        }

        private void UpdateSkillUI(float currentSkill, float maxSkill)
        {
            if (SkillFlowController != null && maxSkill > 0)
            {
                SkillFlowController.SetValue(currentSkill / maxSkill);
            }

            if (SkillImage != null)
            {
                bool availability = currentSkill >= 8 ? true : false;
                SkillImage.SetActive(availability);
            }
        }

        private void UpdateDodgeUI(float currentTime, float maxTime)
        {
            if (dodgeImage != null)
            {
                float fillValue = currentTime / maxTime;
                dodgeImage.fillAmount = Mathf.Clamp01(fillValue);
            }

            if (dodgeText != null)
            {
                float remainingTime = maxTime - currentTime;

                if (remainingTime > 0.01f && remainingTime < maxTime)
                {
                    if (false == dodgeText.gameObject.activeSelf) dodgeText.gameObject.SetActive(true);
                    dodgeText.text = remainingTime.ToString("F1");
                }
                else
                {
                    if (true == dodgeText.gameObject.activeSelf) dodgeText.gameObject.SetActive(false);
                }
            }
        }

        public void UpdateCurrencyUI()
        {
            if (playerStats == null) return;

            if (goldText != null)
            {
                goldText.text = playerStats.CurrentGold.ToString("N0");
            }

            if (upgradeStoneText != null)
            {
                upgradeStoneText.text = playerStats.CurrentUpgradeStones.ToString("N0");
            }
        }

        private void UpdateUpgradeUI(int currentLevel)
        {
            if (weaponLevelText != null)
            {
                weaponLevelText.text = $"+{currentLevel}";
            }
        }
    }
}
