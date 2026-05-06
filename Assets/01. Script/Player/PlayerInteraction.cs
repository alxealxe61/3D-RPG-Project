using _01._Script.Data;
using UnityEngine;
using _01._Script.Item;
using _01._Script.Portal;
using _01._Script.UI;

namespace _01._Script.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("Interaction Settings")]
        [SerializeField] private float interactionRange = 3.0f;
        [SerializeField] private LayerMask interactableLayer; 

        [Header("Debug")]
        [SerializeField] private bool showDebugRay = true;

        private static readonly Vector3 ScreenCenter = new Vector3(0.5f, 0.5f, 0);
        
        private ItemObject _currentItem;
        private Portal.Portal _currentPortal;
        private ScenePortal _currentScenePortal;
        private bool _isLookingAtUpgradeNpc;
            
        public PlayerStats playerStats;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            CheckForInteractable();
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                PerformInteraction();
            }
        }

        private void CheckForInteractable()
        {
            var ray = UnityEngine.Camera.main.ViewportPointToRay(ScreenCenter);
            
            if (Physics.Raycast(ray, out var hit, interactionRange, interactableLayer))
            {
                // 1. 아이템 체크
                if (hit.collider.TryGetComponent<ItemObject>(out var item))
                {
                    if (_currentItem != item)
                    {
                        ClearCurrentTarget();
                        _currentItem = item;
                        InteractionUI.Instance.Show(true);
                    }
                    return;
                }
                
                if (hit.collider.TryGetComponent<Portal.Portal>(out var portal))
                {
                    if (_currentPortal != portal)
                    {
                        ClearCurrentTarget();
                        _currentPortal = portal;
                        InteractionUI.Instance.Show(true);
                    }
                    return;
                }
                
                // 3. 강화 NPC 체크 (NPC 레이어인 경우)
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("NPC"))
                {
                    if (!_isLookingAtUpgradeNpc)
                    {
                        ClearCurrentTarget();
                        _isLookingAtUpgradeNpc = true;
                        InteractionUI.Instance.Show(true);
                    }
                    return;
                }
                
                if (hit.collider.TryGetComponent<ScenePortal>(out var scenePortal))
                {
                    if (_currentScenePortal != scenePortal)
                    {
                        ClearCurrentTarget();
                        _currentScenePortal = scenePortal;
                        InteractionUI.Instance.Show(true);
                    }
                    return;
                }
            }
            
            if (_currentItem != null || _currentPortal != null || _currentScenePortal != null || _isLookingAtUpgradeNpc)
            {
                ClearCurrentTarget();
                InteractionUI.Instance.Show(false);
            }
        }

        private void PerformInteraction()
        {
            if (_currentItem != null)
            {
                if (playerStats != null) playerStats.AddItem(_currentItem.itemType, _currentItem.count);
                _currentItem.OnCollected();
                ClearCurrentTarget();
                InteractionUI.Instance.Show(false);
            }
            else if (_currentPortal != null)
            {
                Teleport(_currentPortal.GetDestinationPosition(), _currentPortal.GetDestinationRotation());
                ClearCurrentTarget();
                InteractionUI.Instance.Show(false);
            }
            else if (_currentScenePortal != null)
            {
                _currentScenePortal.Interact();
                ClearCurrentTarget();
                InteractionUI.Instance.Show(false);
            }
            else if (_isLookingAtUpgradeNpc)
            {
                UIManager.Instance.OpenUpgradeUI();
                ClearCurrentTarget();
                InteractionUI.Instance.Show(false);
            }
        }

        private void ClearCurrentTarget()
        {
            _currentItem = null;
            _currentPortal = null;
            _currentScenePortal = null;
            _isLookingAtUpgradeNpc = false;
        }

        private void Teleport(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
            
            if (TryGetComponent<Rigidbody>(out var rb))
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}
