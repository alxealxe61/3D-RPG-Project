using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EffectData
{
    public string effectName;
    public GameObject effectPrefab;
    public int poolSize = 3;
}

public class EffectManager : SingletonBase<EffectManager>
{
    [SerializeField] private List<EffectData> effectDataList = new List<EffectData>();
    private readonly Dictionary<string, Queue<GameObject>> _pools = new Dictionary<string, Queue<GameObject>>();
    private readonly Dictionary<string, GameObject> _prefabs = new Dictionary<string, GameObject>();

    protected override void OnInitialize()
    {
        foreach (var data in effectDataList)
        {
            if (string.IsNullOrEmpty(data.effectName) || data.effectPrefab == null) continue;
            
            _prefabs[data.effectName] = data.effectPrefab;
            _pools[data.effectName] = new Queue<GameObject>();
            
            for (int i = 0; i < data.poolSize; i++)
            {
                CreateNewEffect(data.effectName);
            }
        }
    }

    private GameObject CreateNewEffect(string effectName)
    {
        var obj = Instantiate(_prefabs[effectName], transform);
        obj.name = effectName;
        if (false == obj.TryGetComponent<PooledEffect>(out var _))
        {
            obj.AddComponent<PooledEffect>();
        }
        obj.SetActive(false);
        _pools[effectName].Enqueue(obj);
        return obj;
    }

    public void PlayEffect(string effectName, Transform parent)
    {
        if (false == _pools.ContainsKey(effectName))
        {
            Debug.LogWarning($"[EffectManager] {effectName} 이름의 이펙트를 찾을 수 없습니다.");
            return;
        }

        GameObject effect = _pools[effectName].Count > 0 
            ? _pools[effectName].Dequeue() 
            : CreateNewEffect(effectName);

        // 프리팹의 로컬 위치/회전/스케일을 유지하며 부모 설정
        //effect.transform.SetParent(parent, false);
        
        effect.SetActive(true);
    }

    /// <summary>
    /// 사용이 끝난 이펙트를 다시 매니저 아래로 수거하고 풀에 넣습니다.
    /// </summary>
    public void ReturnToPool(GameObject effect)
    {
        if (false == _pools.ContainsKey(effect.name))
        {
            Destroy(effect);
            return;
        }

        effect.SetActive(false);
        effect.transform.SetParent(transform); // 부모를 다시 매니저로 변경하여 하이어라키를 정리함
        _pools[effect.name].Enqueue(effect);
    }

    public void StopEffectsUnder(Transform parent)
    {
        var pooledEffects = parent.GetComponentsInChildren<PooledEffect>(true);
        foreach (var effect in pooledEffects)
        {
            if (effect.gameObject.activeSelf)
            {
                ReturnToPool(effect.gameObject);
            }
        }
    }
}
