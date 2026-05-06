using UnityEngine;

namespace _01._Script.Portal
{
    public class Portal : MonoBehaviour
    {
        [Header("Portal Settings")]
        [SerializeField] private Transform destination; // 이동할 목적지 Transform
        [SerializeField] private Vector3 offset = Vector3.up; // 이동 후 위치 오프셋

        public Vector3 GetDestinationPosition()
        {
            if (destination != null)
            {
                return destination.position + offset;
            }
            
            Debug.LogWarning($"[Portal] {gameObject.name}에 목적지(destination)가 설정되지 않았습니다.");
            return transform.position;
        }

        public Quaternion GetDestinationRotation()
        {
            return destination != null ? destination.rotation : transform.rotation;
        }
    }
}
