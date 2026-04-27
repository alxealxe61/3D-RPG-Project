using UnityEngine;
using _01._Script.Item;
using _01._Script.UI;

namespace _01._Script.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("Interaction Settings")]
        [SerializeField] private float interactionRange = 3.0f;
        [SerializeField] private LayerMask itemLayer;
        
        public ItemObject _currentTarget;

        public PlayerStats playerStats;
        private void Update()
        {
            CheckForInteractable();
            
            if (_currentTarget != null && Input.GetKeyDown(KeyCode.E))
            {
                PerformInteraction();
            }
        }

        private void CheckForInteractable()
        {
            // 마우스 위치에서 레이 발사
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, itemLayer))
            {
                if (hit.collider.TryGetComponent<ItemObject>(out var item))
                {
                    float distance = Vector3.Distance(transform.position, item.transform.position);
                    
                    if (distance <= interactionRange)
                    {
                        if (_currentTarget != item)
                        {
                            _currentTarget = item;
                            InteractionUI.Instance.Show(true, item.itemName);
                        }
                        return;
                    }
                }
            }

            // 대상을 찾지 못했거나 범위를 벗어난 경우
            if (_currentTarget != null)
            {
                _currentTarget = null;
                InteractionUI.Instance.Show(false);
            }
        }

        private void PerformInteraction()
        {
            if (_currentTarget == null) return;

            // PlayerStats에 아이템 정보 전달
            if (playerStats != null)
            {
                playerStats.AddItem(_currentTarget.itemType, _currentTarget.count);
            }
            
            Debug.Log($"[아이템 획득] 이름: {_currentTarget.itemName}, 수량: {_currentTarget.count}");

            _currentTarget.OnCollected();
            _currentTarget = null;
            InteractionUI.Instance.Show(false);
        }
    }
}
