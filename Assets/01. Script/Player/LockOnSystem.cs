using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _01._Script.Player
{
    [RequireComponent(typeof(SphereCollider))]
    public class LockOnSystem : MonoBehaviour
    {
        [Header("Tags")]
        public string enemyTag = "Enemy";   // 일반 몬스터 태그
        public string bossTag = "Boss";     // 보스 몬스터 태그
    
        private List<Transform> targetList = new List<Transform>();
        private Dictionary<Transform, Coroutine> _pendingTargets = new Dictionary<Transform, Coroutine>();
        
        [SerializeField] private float targetingDelay = 2.0f;
        
        public Transform CurrentTarget => _currentTarget;
        public bool IsLockedOn => _isLockedOn;

        private Transform _currentTarget;
        private int _currentTargetIndex = 0;
        private bool _isLockedOn = false;

        private void Start()
        {
            var col = GetComponent<Collider>();
            if (col != null) col.isTrigger = true;
        }

        private void Update()
        {
            CleanUpDeadTargets();
        
            if (_isLockedOn)
            {
                if (_currentTarget == null)
                {
                    if (targetList.Count > 0)
                    {
                        SetTarget(0);
                    }
                    else
                    {
                        ReleaseLockOn();
                    }
                }
                else
                {
                    if (Input.GetMouseButtonDown(2))
                    {
                        SwitchTarget();
                    }
                }
            }
        }
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(enemyTag) || other.CompareTag(bossTag))
            {
                if (!targetList.Contains(other.transform) && !_pendingTargets.ContainsKey(other.transform))
                {
                    Coroutine timer = StartCoroutine(AddTargetAfterDelayRoutine(other.transform));
                    _pendingTargets.Add(other.transform, timer);
                }
            }
        }

        private IEnumerator AddTargetAfterDelayRoutine(Transform target)
        {
            yield return new WaitForSeconds(targetingDelay);

            if (target != null)
            {
                if (!targetList.Contains(target))
                {
                    targetList.Add(target);
                }
                _pendingTargets.Remove(target);

                if (_isLockedOn == false && targetList.Count == 1)
                {
                    SetTarget(0);
                    _isLockedOn = true;
                }
            }
        }
    
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(enemyTag) || other.CompareTag(bossTag))
            {
                if (_pendingTargets.TryGetValue(other.transform, out Coroutine timer))
                {
                    StopCoroutine(timer);
                    _pendingTargets.Remove(other.transform);
                }

                if (targetList.Contains(other.transform))
                {
                    targetList.Remove(other.transform);
                
                    if (_isLockedOn && _currentTarget == other.transform)
                    {
                        if (targetList.Count > 0)
                        {
                            SetTarget(0); 
                        }
                        else
                        {
                            ReleaseLockOn(); 
                        }
                    }
                }
            }
        }
    
        private void SwitchTarget()
        {
            if (_currentTarget == null || targetList.Count <= 1) return;
        
            if (_currentTarget.CompareTag(bossTag))
            {
                return;
            }
        
            _currentTargetIndex = (_currentTargetIndex + 1) % targetList.Count;
            SetTarget(_currentTargetIndex);
        }
    
        private void SetTarget(int index)
        {
            if (index < 0 || index >= targetList.Count) return;
            _currentTargetIndex = index;
            _currentTarget = targetList[_currentTargetIndex];
        }
    
        private void ReleaseLockOn()
        {
            _isLockedOn = false;
            _currentTarget = null;
            _currentTargetIndex = 0;
        }
    
        private void CleanUpDeadTargets()
        {
            var deadPending = new List<Transform>();
            foreach (var pending in _pendingTargets.Keys)
            {
                if (pending == null || !pending.gameObject.activeInHierarchy)
                    deadPending.Add(pending);
            }
            foreach (var dead in deadPending)
            {
                if (_pendingTargets.TryGetValue(dead, out Coroutine timer)) StopCoroutine(timer);
                _pendingTargets.Remove(dead);
            }

            targetList.RemoveAll(target => target == null || !target.gameObject.activeInHierarchy);
        }
    }
}
