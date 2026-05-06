using System.Collections;
using _01._Script.Data;
using _01._Script.UpgradeWeapon_System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _01._Script.UI
{
    public class UpgradeUI : MonoBehaviour
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
        [SerializeField] private TextMeshProUGUI probabilityText; // 강화 확률 표시 추가

        [Header("--- Control UI (Bottom) ---")]
        [SerializeField] private Button upgradeButton;
        [SerializeField] private TextMeshProUGUI resultMessageText;

        [Header("--- Settings ---")]
        [SerializeField] private float messageDisplayDuration = 2.0f;
        [SerializeField] private Color successColor = Color.yellow;
        [SerializeField] private Color failureColor = Color.red;

        private bool _isProcessing = false;

        private void Awake()
        {
            if (upgradeSystem == null) upgradeSystem = FindFirstObjectByType<UpgradeSystem>();
            if (playerStats == null) playerStats = FindFirstObjectByType<PlayerStats>();

            if (upgradeButton != null)
                upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
            
            if (resultMessageText != null) 
                resultMessageText.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            // 씬에 시스템이 없을 경우를 대비해 다시 찾음
            if (upgradeSystem == null) upgradeSystem = FindFirstObjectByType<UpgradeSystem>();
            
            SubscribeEvents();
            RefreshUI();
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
            _isProcessing = false;
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
                playerStats.OnUpgradeChanged += HandleUpgradeChanged;
            }
        }

        private void UnsubscribeEvents()
        {
            if (upgradeSystem != null)
            {
                upgradeSystem.OnUpgradeSuccess -= HandleUpgradeSuccess;
                upgradeSystem.OnUpgradeFailure -= HandleUpgradeFailure;
                upgradeSystem.OnUpgradeError -= HandleUpgradeError;
            }

            if (playerStats != null)
            {
                playerStats.OnCurrencyChanged -= RefreshUI;
                playerStats.OnUpgradeChanged -= HandleUpgradeChanged;
            }
        }

        private void HandleUpgradeChanged(int level) => RefreshUI();

        private void RefreshUI()
        {
            if (playerStats == null) return;

            int currentLevel = playerStats.CurrentWeaponLevel;

            // 재료 정보 갱신
            if (goldCostText != null) goldCostText.text = $" 골드 : {UpgradeSystem.UpgradeCostGold}";
            if (stoneCostText != null) stoneCostText.text = $" 강화석 : {UpgradeSystem.UpgradeCostStone}";
            
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

            // 확률 정보 갱신
            if (probabilityText != null && upgradeSystem != null)
            {
                if (currentLevel < UpgradeSystem.MaxLevel)
                {
                    var rate = upgradeSystem.GetCurrentSuccessRate();
                    probabilityText.text = $"강화 확률 : {rate}%";
                }
                else
                {
                    probabilityText.text = "최대 강화 완료";
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
            if (_isProcessing) return;
            
            if (upgradeSystem != null)
            {
                // 버튼 즉시 비활성화
                if (upgradeButton != null) upgradeButton.interactable = false;
                _isProcessing = true;
                
                upgradeSystem.TryUpgrade();
            }
        }

        private void HandleUpgradeSuccess()
        {
            StartCoroutine(ShowResultMessageRoutine("강화 성공!", successColor));
        }

        private void HandleUpgradeFailure()
        {
            StartCoroutine(ShowResultMessageRoutine("강화 실패...", failureColor));
        }

        private void HandleUpgradeError(string message)
        {
            StartCoroutine(ShowResultMessageRoutine(message, failureColor));
        }

        private IEnumerator ShowResultMessageRoutine(string message, Color color)
        {
            if (resultMessageText != null)
            {
                resultMessageText.text = message;
                resultMessageText.color = color;
                resultMessageText.gameObject.SetActive(true);
            }

            yield return new WaitForSeconds(messageDisplayDuration);

            if (resultMessageText != null)
            {
                resultMessageText.gameObject.SetActive(false);
            }
            
            _isProcessing = false;
            RefreshUI();
        }
    }
}
