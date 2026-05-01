using UnityEngine;
using TMPro;

namespace _01._Script.UI
{
    public class InteractionUI : SingletonBase<InteractionUI>
    {
        [SerializeField] private GameObject uiPanel; // E키 안내 판넬
        [SerializeField] private TextMeshProUGUI infoText; // 문구 텍스트

        [Header("Follow Settings")]
        [SerializeField] private Vector2 offset = new Vector2(100f, 50f); // 마우스 커서와의 간격

        protected override void OnInitialize()
        {
            base.OnInitialize();
            Show(false);
        }

        private void Update()
        {
            // UI가 활성화된 상태에서만 마우스 위치를 따라갑니다.
            if (uiPanel != null && uiPanel.activeSelf)
            {
                UpdatePosition();
            }
        }

        private void UpdatePosition()
        {
            Vector2 mousePos = Input.mousePosition;
            uiPanel.transform.position = mousePos + offset;
        }

        public void Show(bool isShow)
        {
            if (uiPanel == null) return;
            
            uiPanel.SetActive(isShow);
            
            if (isShow)
            {
                if (infoText != null)
                {
                    infoText.text = $"<color=yellow>E</color>";
                }
                
                // 표시되는 즉시 위치 업데이트하여 튀는 현상 방지
                UpdatePosition();
            }
        }
    }
}
