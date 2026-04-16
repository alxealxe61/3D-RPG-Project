using UnityEngine;

namespace _01._Script
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform cameraTarget;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private Transform headPivotTransform;
        [SerializeField] private LockOnSystem lockOnSystem;
        
        [SerializeField] private float maxAngleX = 90;
        [SerializeField] private float minAngleX = -90;
        
        [SerializeField] private float horizontalSensitivity = 1.0f;
        [SerializeField] private float verticalSensitivity = 1.0f;
        [SerializeField] private float lockOnRotationSpeed = 10.0f;
        
        private Vector2 currentAngle = Vector2.zero; 
        
        void Update()
        {
            if (lockOnSystem != null && lockOnSystem.IsLockedOn && lockOnSystem.CurrentTarget != null)
            {
                HandleLockOnRotation();
            }
            else
            {
                UpdateRotation();
            }
        }
        
        private void UpdateRotation()
        {
            Vector2 mouseInput = new Vector2(
                Input.GetAxis("Mouse X") * horizontalSensitivity, 
                Input.GetAxis("Mouse Y") * verticalSensitivity);
            
            currentAngle.x -= mouseInput.y;
            currentAngle.x = Mathf.Clamp(currentAngle.x, minAngleX, maxAngleX);
            
            transform.Rotate(Vector3.up, mouseInput.x);
            headPivotTransform.localRotation = Quaternion.Euler(currentAngle.x, 0, 0);
        }

        private void HandleLockOnRotation()
        {
            // 타겟 방향 계산 (카메라가 타겟을 바라보도록)
            Vector3 targetDir = (lockOnSystem.CurrentTarget.position - cameraTransform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(targetDir);
            
            // 전체 리액트(transform)는 좌우 회전만, 헤드 피벗은 상하 회전만 담당하도록 분리 가능하지만,
            // 간단하게 카메라 자체가 타겟을 부드럽게 바라보도록 처리합니다.
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, targetRotation, Time.deltaTime * lockOnRotationSpeed);
            
            // 록온 중에는 현재 각도 데이터를 초기화하여 비록온 전환 시 튀지 않게 함
            currentAngle.x = cameraTransform.localEulerAngles.x;
            if (currentAngle.x > 180) currentAngle.x -= 360;
        }
    }
}
