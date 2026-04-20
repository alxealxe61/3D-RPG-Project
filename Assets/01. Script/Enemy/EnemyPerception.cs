using System;
using System.Collections.Generic;
using UnityEngine;

namespace _01._Script.Enemy
{
    public class EnemyPerception : MonoBehaviour
    {
        public event Action<Transform> OnTargetDetected;

        public event Action OnTargetLost;

        [SerializeField] private string targetTag = "Player";
        
        private Transform detectedTarget;
            
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