using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SphereCollider))]
public class LockOnSystem : MonoBehaviour
{
    [Header("Tags")]
    public string enemyTag = "Enemy";   // 일반 몬스터 태그
    public string bossTag = "Boss";     // 보스 몬스터 태그
    
    private List<Transform> targetList = new List<Transform>();
    
   
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
            if (!targetList.Contains(other.transform))
            {
                targetList.Add(other.transform);
                
                if (_isLockedOn == false && targetList.Count == 1)
                {
                    SetTarget(0);
                    _isLockedOn = true;
                }
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(enemyTag) || other.CompareTag(bossTag))
        {
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
        targetList.RemoveAll(target => target == null || !target.gameObject.activeInHierarchy);
    }
}     