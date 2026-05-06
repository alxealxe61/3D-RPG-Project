using System;
using UnityEngine;

namespace _01._Script.Enemy
{
    public class AttackRange : MonoBehaviour
    {
        public bool IsInAttackRange { get; private set; }

        [SerializeField] private string targetTag = "Player";

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(targetTag))
            {
                IsInAttackRange = true;
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