using _01._Script.Data;
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
        [SerializeField] private LayerMask interactableLayer; 

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
            var ray = Camera.main.ViewportPointToRay(ScreenCenter);
            
            if (Physics.Raycast(ray, out var hit, interactionRange, interactableLayer))
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
            
            if (_currentItem != null || _currentPortal != null || _currentScenePortal != null)
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
        }

        private void OnDrawGizmos()
        {
            if (showDebugRay == false || Camera.main == null) return;

            Gizmos.color = debugRayColor;
            var ray = Camera.main.ViewportPointToRay(ScreenCenter);
            var drawDistance = interactionRange;

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
