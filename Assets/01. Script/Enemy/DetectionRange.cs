using System;
using System.Collections.Generic;
using UnityEngine;

namespace _01._Script.Enemy
{
    public class DetectionRange : MonoBehaviour
    {
        public event Action<Transform> OnTargetDetected;
        public event Action OnTargetLost;

        [SerializeField] private string targetTag = "Player";

        public Transform detectedTarget;
            
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(targetTag))
            {
                detectedTarget = other.transform;
                OnTargetDetected?.Invoke(detectedTarget);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(targetTag))
            {
                if (other.transform == detectedTarget)
                {
                    detectedTarget = null;
                    OnTargetLost?.Invoke();
                }
            }
        }

        private void OnDisable()
        {
            detectedTarget = null;
        }
    }
}