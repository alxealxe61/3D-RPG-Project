using _01._Script.Camera;
using _01._Script.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _01._Script.UI
{
    public class UIManager : SingletonBase<UIManager>
    {
        [Header("--- UI Panels ---")]
        [SerializeField] private GameObject inGameUI;     // 게임 플레이 중 UI
        [SerializeField] private GameObject menuUI;       // ESC 메뉴 패널
        [SerializeField] private GameObject saveSlotPanel; // 세이브 슬롯 패널
        [SerializeField] private GameObject dieUI;             // 사망 UI 패널
        [SerializeField] private GameObject upgradeUI;

        [Header("--- External References ---")]
        [SerializeField] private CameraController cameraController; // 카메라 컨트롤러 참조 추가

        [Header("--- Menu Buttons ---")]
        [SerializeField] private Button continueButton;
        [SerializeField] private Button saveMenuButton;
        [SerializeField] private Button quitButton;

        [Header("--- Options ---")]
        [SerializeField] private bool canUseMenu = true; // 메뉴 사용 가능 여부

        [Header("Game Over Buttons")]
        [SerializeField] private Button returnToVillageButton;
        [SerializeField] private Button quitGameButton;
        
        [Header("Settings")]
        [SerializeField] private string villageSceneName = "01.Village";
        
        private bool isMenuOpen = false;

        protected override void OnInitialize()
        {
            base.OnInitialize();
            
            // 패널들이 UIManager의 자식이 아닐 경우를 대비해 DontDestroyOnLoad 명시적 호출
            if (inGameUI != null) DontDestroyOnLoad(inGameUI);
            if (menuUI != null) DontDestroyOnLoad(menuUI);
            if (saveSlotPanel != null) DontDestroyOnLoad(saveSlotPanel);
            if (dieUI != null) DontDestroyOnLoad(dieUI);
            if (upgradeUI != null) DontDestroyOnLoad(upgradeUI);
        }

        private void Start()
        {
            // 카메라 컨트롤러 자동 찾기 (할당 안 했을 경우)
            if (cameraController == null)
                cameraController = FindFirstObjectByType<CameraController>();

            // 게임 시작 시 초기 상태: 커서 숨김 및 카메라 활성화
            ResumeGame();

            // 버튼 이벤트 연결
            if (continueButton != null) continueButton.onClick.AddListener(ResumeGame);
            if (saveMenuButton != null) saveMenuButton.onClick.AddListener(OpenSaveMenu);
            if (quitButton != null) quitButton.onClick.AddListener(QuitGame);
            if (returnToVillageButton != null) returnToVillageButton.onClick.AddListener(OnReturnToVillageClicked);
            if (quitGameButton != null) quitGameButton.onClick.AddListener(QuitGame);
        }

        private void Update()
        {
            // 메뉴 사용이 불가능하거나 이미 사망 UI가 떠있으면 ESC 로직을 건너뜜
            if (canUseMenu == false || (dieUI != null && dieUI.gameObject.activeSelf)) return;

            // ESC 키 감지
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // 강화창이 열려있으면 강화창부터 닫음
                if (upgradeUI != null && upgradeUI.activeSelf)
                {
                    CloseUpgradeUI();
                    return;
                }

                if (!isMenuOpen)
                    OpenMenu();
                else
                    ResumeGame();
            }
        }

        private void OpenMenu()
        {
            isMenuOpen = true;
            Time.timeScale = 0f; // 게임 일시정지

            if (inGameUI != null) inGameUI.SetActive(false);
            if (menuUI != null) menuUI.SetActive(true);
            if (saveSlotPanel != null) saveSlotPanel.SetActive(false);

            // 커서 활성화
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            // 카메라 회전 중지
            if (cameraController != null) cameraController.enabled = false;
        }

        private void ResumeGame()
        {
            isMenuOpen = false;
            Time.timeScale = 1f; // 게임 재개

            if (inGameUI != null) inGameUI.SetActive(true);
            if (menuUI != null) menuUI.SetActive(false);
            if (saveSlotPanel != null) saveSlotPanel.SetActive(false);

            // 커서 숨김 (플레이 모드)
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            // 카메라 회전 재개
            if (cameraController != null) cameraController.enabled = true;
        }

        public void ShowDieUI()
        {
            if (dieUI != null)
            {
                // 인게임 UI 숨기기
                if (inGameUI != null) inGameUI.SetActive(false);
                
                // 카메라 회전 중지
                if (cameraController != null) cameraController.enabled = false;

                dieUI.SetActive(true);
                Show();
            }
            else
            {
                Debug.LogWarning("[UIManager] DieUI가 할당되지 않았습니다.");
            }
        }

        private void OpenSaveMenu()
        {
            if (menuUI != null) menuUI.SetActive(false);
            if (saveSlotPanel != null)
            {
                saveSlotPanel.SetActive(true);
                // 슬롯 프리뷰 갱신
                var slots = saveSlotPanel.GetComponentsInChildren<SaveSlotUI>(true);
                foreach (var slot in slots) slot.RefreshSlot();
            }
        }
        
        public void OpenUpgradeUI()
        {
            if (upgradeUI == null) return;
            if (inGameUI != null) inGameUI.SetActive(false);
            upgradeUI.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            // 카메라 회전 중지
            if (cameraController != null) cameraController.enabled = false;
        }

        private void CloseUpgradeUI()
        {
            if (upgradeUI == null) return;

            if (inGameUI != null) inGameUI.SetActive(true);
            upgradeUI.SetActive(false);
            ResumeGame();
        }

        private void Show()
        {
            // 시간 정지 및 커서 활성화
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        private void OnReturnToVillageClicked()
        {
            Time.timeScale = 1f;
            
            // 플레이어 상태 및 체력 초기화
            if (PlayerController.Instance != null)
            {
                PlayerController.Instance.ResetState();
                if (PlayerController.Instance.playerStats != null)
                {
                    PlayerController.Instance.playerStats.FullRecover();
                }
            }

            // SceneTransitionManager를 통해 마을로 이동 (기본 포탈 ID "Default" 가정)
            if (SceneTransitionManager.Instance != null)
            {
                SceneTransitionManager.Instance.TransitionToScene(villageSceneName, "Shop Portal");
            }
            else
            {
                SceneManager.LoadScene(villageSceneName);
            }
            
            if (inGameUI != null) inGameUI.SetActive(true);
            if (menuUI != null) menuUI.SetActive(false);
            if (saveSlotPanel != null) saveSlotPanel.SetActive(false);
            if (dieUI != null) dieUI.gameObject.SetActive(false);
            
            if (cameraController != null) cameraController.enabled = true;
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
