using System.Collections;
using _01._Script.Data;
using _01._Script.UpgradeWeapon_System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _01._Script.UI
{
    public class UpgradeWindowUI : MonoBehaviour
    {
        [Header("--- Systems ---")]
        [SerializeField] private UpgradeSystem upgradeSystem;
        [SerializeField] private PlayerStats playerStats;

        [Header("--- Material UI (Top) ---")]
        [SerializeField] private TextMeshProUGUI goldCostText;
        [SerializeField] private TextMeshProUGUI stoneCostText;
        [SerializeField] private TextMeshProUGUI currentGoldText;
        [SerializeField] private TextMeshProUGUI currentStoneText;

        [Header("--- Level UI (Middle) ---")]
        [SerializeField] private TextMeshProUGUI currentLevelText;
        [SerializeField] private TextMeshProUGUI nextLevelText;

        [Header("--- Control UI (Bottom) ---")]
        [SerializeField] private Button upgradeButton;
        [SerializeField] private TextMeshProUGUI resultMessageText;

        [Header("--- Settings ---")]
        [SerializeField] private float messageDisplayDuration = 3.0f;
        [SerializeField] private Color successColor = Color.yellow;
        [SerializeField] private Color failureColor = Color.red;

        private bool _isProcessing = false;

        private void Awake()
        {
            if (upgradeSystem == null) upgradeSystem = FindAnyObjectByType<UpgradeSystem>();
            if (playerStats == null) playerStats = FindAnyObjectByType<PlayerStats>();

            if (upgradeButton != null)
                upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
            
            if (resultMessageText != null) 
                resultMessageText.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            SubscribeEvents();
            RefreshUI();
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            if (upgradeSystem != null)
            {
                upgradeSystem.OnUpgradeSuccess += HandleUpgradeSuccess;
                upgradeSystem.OnUpgradeFailure += HandleUpgradeFailure;
                upgradeSystem.OnUpgradeError += HandleUpgradeError;
            }

            if (playerStats != null)
            {
                playerStats.OnCurrencyChanged += RefreshUI;
                playerStats.OnUpgradeChanged += _ => RefreshUI();
            }
        }

        private void UnsubscribeEvents()
        {
            if (upgradeSystem != null)
            {
                upgradeSystem.OnUpgradeSuccess -= HandleUpgradeSuccess;
                upgradeSystem.OnUpgradeFailure -= HandleFailure;
                upgradeSystem.OnUpgradeError -= HandleError;
            }

            if (playerStats != null)
            {
                playerStats.OnCurrencyChanged -= RefreshUI;
                playerStats.OnUpgradeChanged -= _ => RefreshUI();
            }
        }

        // UnsubscribeEvents에서 사용하는 래퍼 메서드 (타입 일치를 위해)
        private void HandleFailure() => HandleUpgradeFailure();
        private void HandleError(string msg) => HandleUpgradeError(msg);

        public void RefreshUI()
        {
            if (playerStats == null) return;

            int currentLevel = playerStats.CurrentWeaponLevel;

            // 재료 정보 갱신
            if (goldCostText != null) goldCostText.text = $"{UpgradeSystem.UpgradeCostGold}";
            if (stoneCostText != null) stoneCostText.text = $"{UpgradeSystem.UpgradeCostStone}";
            
            if (currentGoldText != null) currentGoldText.text = $"{playerStats.CurrentGold}";
            if (currentStoneText != null) currentStoneText.text = $"{playerStats.CurrentUpgradeStones}";

            // 레벨 정보 갱신
            if (currentLevelText != null) currentLevelText.text = $"+{currentLevel}";
            
            if (nextLevelText != null)
            {
                if (currentLevel < UpgradeSystem.MaxLevel)
                {
                    nextLevelText.text = $"+{currentLevel + 1}";
                }
                else
                {
                    nextLevelText.text = "MAX";
                }
            }

            // 버튼 상태 제어
            if (!_isProcessing && upgradeButton != null)
            {
                upgradeButton.interactable = currentLevel < UpgradeSystem.MaxLevel;
            }
        }

        private void OnUpgradeButtonClicked()
        {
            if (upgradeSystem != null)
            {
                upgradeSystem.TryUpgrade();
            }
        }

        private void HandleUpgradeSuccess()
        {
            StartCoroutine(ShowResultMessageRoutine("강화성공!", successColor));
        }

        private void HandleUpgradeFailure()
        {
            StartCoroutine(ShowResultMessageRoutine("강화실패....", failureColor));
        }

        private void HandleUpgradeError(string message)
        {
            StartCoroutine(ShowResultMessageRoutine(message, failureColor, false));
        }

        private IEnumerator ShowResultMessageRoutine(string message, Color color, bool disableButton = true)
        {
            if (resultMessageText == null) yield break;

            _isProcessing = true;
            resultMessageText.text = message;
            resultMessageText.color = color;
            resultMessageText.gameObject.SetActive(true);

            if (disableButton && upgradeButton != null)
            {
                upgradeButton.interactable = false;
            }

            yield return new WaitForSeconds(messageDisplayDuration);

            resultMessageText.gameObject.SetActive(false);
            _isProcessing = false;

            RefreshUI();
        }
    }
}
