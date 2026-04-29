using UnityEngine;
using _01._Script.Item;
using _01._Script.UI;
using _01._Script.Environment;

namespace _01._Script.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("Interaction Settings")]
        [SerializeField] private float interactionRange = 3.0f;
        [SerializeField] private LayerMask interactableLayer; // 아이템과 포탈 레이어를 모두 포함

        [Header("Debug")]
        [SerializeField] private bool showDebugRay = true;
        [SerializeField] private Color debugRayColor = Color.green;
        [SerializeField] private Color debugHitColor = Color.red;

        private static readonly Vector3 ScreenCenter = new Vector3(0.5f, 0.5f, 0);
        private const float HitMarkerSize = 0.1f;
        
        private ItemObject _currentItem;
        private Portal _currentPortal;
        private ScenePortal _currentScenePortal;

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
            Ray ray = Camera.main.ViewportPointToRay(ScreenCenter);
            
            if (Physics.Raycast(ray, out RaycastHit hit, interactionRange, interactableLayer))
            {
                // 1. 아이템 체크
                if (hit.collider.TryGetComponent<ItemObject>(out var item))
                {
                    if (_currentItem != item)
                    {
                        ClearCurrentTarget();
                        _currentItem = item;
                        InteractionUI.Instance.Show(true, item.itemName);
                    }
                    return;
                }
                
                // 2. 포탈 체크
                if (hit.collider.TryGetComponent<Portal>(out var portal))
                {
                    if (_currentPortal != portal)
                    {
                        ClearCurrentTarget();
                        _currentPortal = portal;
                        InteractionUI.Instance.Show(true, portal.portalName);
                    }
                    return;
                }

                // 3. 씬 포탈 체크
                if (hit.collider.TryGetComponent<ScenePortal>(out var scenePortal))
                {
                    if (_currentScenePortal != scenePortal)
                    {
                        ClearCurrentTarget();
                        _currentScenePortal = scenePortal;
                        InteractionUI.Instance.Show(true, scenePortal.portalName);
                    }
                    return;
                }
            }

            // 아무것도 감지되지 않았을 때
            if (_currentItem != null || _currentPortal != null || _currentScenePortal != null)
            {
                ClearCurrentTarget();
                InteractionUI.Instance.Show(false);
            }
        }

        private void PerformInteraction()
        {
            // 아이템 획득
            if (_currentItem != null)
            {
                if (playerStats != null) playerStats.AddItem(_currentItem.itemType, _currentItem.count);
                _currentItem.OnCollected();
                ClearCurrentTarget();
                InteractionUI.Instance.Show(false);
            }
            // 포탈 이동
            else if (_currentPortal != null)
            {
                Teleport(_currentPortal.GetDestinationPosition(), _currentPortal.GetDestinationRotation());
                // 이동 후에는 타겟 해제 (이동하자마자 다시 E가 뜨는 것 방지)
                ClearCurrentTarget();
                InteractionUI.Instance.Show(false);
            }
            // 씬 포탈 이동
            else if (_currentScenePortal != null)
            {
                _currentScenePortal.Interact();
                ClearCurrentTarget();
                InteractionUI.Instance.Show(false);
            }
        }

        private void ClearCurrentTarget()
        {
            _currentItem = null;
            _currentPortal = null;
            _currentScenePortal = null;
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

            Debug.Log($"[Teleport] {position} 위치로 이동했습니다.");
        }

        private void OnDrawGizmos()
        {
            if (showDebugRay == false || Camera.main == null) return;

            Gizmos.color = debugRayColor;
            Ray ray = Camera.main.ViewportPointToRay(ScreenCenter);
            float drawDistance = interactionRange;

            if (Physics.Raycast(ray, out RaycastHit hit, interactionRange, interactableLayer))
            {
                drawDistance = hit.distance;
                Gizmos.color = debugHitColor;
                Gizmos.DrawSphere(hit.point, HitMarkerSize);
            }

            Gizmos.DrawRay(ray.origin, ray.direction * drawDistance);
        }
    }
}
