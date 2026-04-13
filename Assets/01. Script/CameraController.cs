using UnityEngine;

namespace _01._Script
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform cameraTarget;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private Transform headPivotTransform;
        
        [SerializeField] private float maxAngleX = 90;
        [SerializeField] private float minAngleX = -90;
        
        [SerializeField] private float horizontalSensitivity = 1.0f;
        [SerializeField] private float verticalSensitivity = 1.0f;
        
        private Vector2 currentAngle = Vector2.zero; 
        
        void Update()
        {
            UpdateRotation();
        }
        
        private void UpdateRotation()
        {
            Vector2 mouseInput = new Vector2(
                Input.GetAxis("Mouse X") * horizontalSensitivity, 
                Input.GetAxis("Mouse Y") * verticalSensitivity);
            
            currentAngle.x -= mouseInput.y;
            
            transform.Rotate(Vector3.up, mouseInput.x);
            currentAngle.y = 0.0f;
            
            float mouseX = Input.GetAxis("Mouse X") * horizontalSensitivity;
            transform.Rotate(Vector3.up * mouseX);
            
            currentAngle.x = Mathf.Clamp(currentAngle.x, minAngleX, maxAngleX);
            headPivotTransform.localRotation = Quaternion.Euler(currentAngle);
        }
    }
}
