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
    
        private readonly List<Transform> _targetList = new List<Transform>();
        private readonly Dictionary<Transform, Coroutine> _pendingTargets = new Dictionary<Transform, Coroutine>();
        
        //[SerializeField] private float targetingDelay = 1.0f;
        
        public Transform CurrentTarget { get; private set; }

        public bool IsLockedOn { get; private set; }

        private int _currentTargetIndex;

        private void Start()
        {
            var col = GetComponent<Collider>();
            if (col != null) col.isTrigger = true;
        }

        private void Update()
        {
            CleanUpDeadTargets();
        
            if (IsLockedOn)
            {
                if (CurrentTarget == null)
                {
                    if (_targetList.Count > 0)
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
                if (!_targetList.Contains(other.transform) && !_pendingTargets.ContainsKey(other.transform))
                {
                    var timer = StartCoroutine(AddTargetAfterDelayRoutine(other.transform));
                    _pendingTargets.Add(other.transform, timer);
                }
            }
        }

        private IEnumerator AddTargetAfterDelayRoutine(Transform target)
        {
            yield return new WaitForSeconds(1);

            if (target != null)
            {
                if (!_targetList.Contains(target))
                {
                    _targetList.Add(target);
                }
                _pendingTargets.Remove(target);

                if (IsLockedOn == false && _targetList.Count == 1)
                {
                    SetTarget(0);
                    IsLockedOn = true;
                }
            }
        }
    
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(enemyTag) || other.CompareTag(bossTag))
            {
                if (_pendingTargets.TryGetValue(other.transform, out var timer))
                {
                    StopCoroutine(timer);
                    _pendingTargets.Remove(other.transform);
                }

                if (_targetList.Contains(other.transform))
                {
                    _targetList.Remove(other.transform);
                
                    if (IsLockedOn && CurrentTarget == other.transform)
                    {
                        if (_targetList.Count > 0)
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
            if (CurrentTarget == null || _targetList.Count <= 1) return;
        
            if (CurrentTarget.CompareTag(bossTag))
            {
                return;
            }
        
            _currentTargetIndex = (_currentTargetIndex + 1) % _targetList.Count;
            SetTarget(_currentTargetIndex);
        }
    
        private void SetTarget(int index)
        {
            if (index < 0 || index >= _targetList.Count) return;
            _currentTargetIndex = index;
            CurrentTarget = _targetList[_currentTargetIndex];
        }
    
        private void ReleaseLockOn()
        {
            IsLockedOn = false;
            CurrentTarget = null;
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
                if (_pendingTargets.TryGetValue(dead, out var timer)) StopCoroutine(timer);
                _pendingTargets.Remove(dead);
            }

            _targetList.RemoveAll(target => target == null || !target.gameObject.activeInHierarchy);
        }
    }
}
