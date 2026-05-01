using System;
using _01._Script.Data;
using _01._Script.Effect;
using UnityEngine;

namespace _01._Script.UpgradeWeapon_System
{
    public class UpgradeSystem : MonoBehaviour
    {
        [Header("--- References ---")]
        [SerializeField] private PlayerStats playerStats;

        [Header("--- Upgrade Settings (1~5 Level) ---")]
        public const int UpgradeCostGold = 500;
        public const int UpgradeCostStone = 10;
        private const int AttackIncreaseAmount = 2;
        public const int MaxLevel = 15;

        // 성공 확률 (Index 0: 0->1강, Index 1: 1->2강 ...)
        private readonly int[] _successRates = { 100, 95, 90, 85, 80, 75,70,60,55,50,40,30,15,10,5 };

        // --- [UI 연동을 위한 이벤트] ---
        public event Action OnUpgradeSuccess;
        public event Action OnUpgradeFailure;
        public event Action<string> OnUpgradeError;

        private void Awake()
        {
            if (playerStats == null)
            {
                playerStats = FindAnyObjectByType<PlayerStats>();
            }
            DontDestroyOnLoad(gameObject);
        }
        
        /// <summary>
        /// 무기 강화를 시도합니다.
        /// </summary>
        public void TryUpgrade()
        {
            int currentLevel = playerStats.CurrentWeaponLevel;

            if (currentLevel >= MaxLevel)
            {
                OnUpgradeError?.Invoke("최고 강화 단계에 도달했습니다.");
                return;
            }

            // 재화 소모 확인
            if (!playerStats.SpendResources(UpgradeCostGold, UpgradeCostStone))
            {
                OnUpgradeError?.Invoke("강화에 필요한 재화가 부족합니다.");
                return;
            }

            // 확률 계산
            int successRate = _successRates[currentLevel];
            int randomValue = UnityEngine.Random.Range(0, 100);

            if (randomValue < successRate)
            {
                // 강화 성공
                playerStats.UpgradeWeapon(AttackIncreaseAmount);
                
                // 연출 실행
                SoundManager.Instance.PlaySFX("UpgradeSuccess", transform.position);
                EffectManager.Instance.PlayEffect("UpgradeSuccessEffect", transform);
                
                // 사운드
                //OnUpgradeSuccess?.Invoke();
                
                // 성공 시 자동 저장
                //DataManager.Instance.SaveGame(0); 
            }
            else
            {
                // 강화 실패
                SoundManager.Instance.PlaySFX("UpgradeFail", transform.position);
                // 사운드
                //OnUpgradeFailure?.Invoke();
            }
        }
    }
}
