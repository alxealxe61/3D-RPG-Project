using UnityEngine;
using TMPro;
using UnityEngine.UI;
using _01._Script;
using CrusaderUI.Scripts;
using UnityEngine.Serialization;

namespace _01._Script.UI_Manager
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

        [Header("Dodge UI")]
        [SerializeField] private TextMeshProUGUI dodgeText;
        [SerializeField] private Image dodgeImage;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            // 1. PlayerStats 찾기 시도
            if (playerStats == null)
            {
                playerStats = FindFirstObjectByType<PlayerStats>();
            }

            if (playerStats != null)
            {
                SubscribeEvents();
                
                // 2. 시작 시 모든 UI 강제 초기화 (이벤트 발생을 기다리지 않고 현재 값을 즉시 반영)
                RefreshAllUI();
            }
            else
            {
                Debug.LogWarning("[UserInfoUI] PlayerStats를 찾을 수 없습니다. UI가 갱신되지 않습니다.");
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

                if (playerController != null)
                {
                    playerController.OnDodgeCooldownChanged -= UpdateDodgeUI;
                }
            }
        }

        /// <summary>
        /// 모든 UI 요소를 현재 PlayerStats 데이터로 즉시 갱신합니다.
        /// </summary>
        private void RefreshAllUI()
        {
            UpdateHpUI(playerStats.CurrentHp, playerStats.MaxHp);
            UpdateSkillUI(playerStats.currentSkillPoint, 20f); // Max 스킬 포인트는 20 고정
            UpdateCurrencyUI();
            
            if (playerController != null)
            {
                UpdateDodgeUI(playerController.dodgetime, PlayerController.DODGE_COOLDOWN);
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
    }
}
