using System;
using UnityEngine;

namespace _01._Script.Enemy
{
    public class AttackRange : MonoBehaviour
    {
        public event Action OnTargetInAttackRange;
        public bool IsInAttackRange { get; private set; } = false;

        [SerializeField] private string targetTag = "Player";

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(targetTag))
            {
                IsInAttackRange = true;
                OnTargetInAttackRange?.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(targetTag))
            {
                IsInAttackRange = false;
            }
        }
    }
}