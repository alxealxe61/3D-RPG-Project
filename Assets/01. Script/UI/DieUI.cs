using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using _01._Script.UI;

namespace _01._Script.UI
{
    public class DieUI : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button returnToVillageButton;
        [SerializeField] private Button quitGameButton;

        [Header("Settings")]
        [SerializeField] private string villageSceneName = "01.Village";

        private void Start()
        {
            if (returnToVillageButton != null)
                returnToVillageButton.onClick.AddListener(OnReturnToVillageClicked);

            if (quitGameButton != null)
                quitGameButton.onClick.AddListener(OnQuitGameClicked);

            // 초기에는 비활성화
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            
            // 시간 정지 및 커서 활성화
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            
            // 카메라 컨트롤러 등이 있다면 여기서 비활성화 처리가 필요할 수 있습니다.
            // UIManager의 기능을 활용하는 것이 좋습니다.
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
                SceneTransitionManager.Instance.TransitionToScene(villageSceneName, "Default");
            }
            else
            {
                SceneManager.LoadScene(villageSceneName);
            }
        }

        private void OnQuitGameClicked()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
