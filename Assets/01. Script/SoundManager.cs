using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SoundData
{
    public string soundName;
    public AudioClip clip;
    [Range(0, 1)] public float volume = 1.0f;
}

public class SoundManager : SingletonBase<SoundManager>
{
    [Header("SFX Settings")]
    [SerializeField] private List<SoundData> sfxDataList = new List<SoundData>();
    [SerializeField] private int sfxPoolCount = 10;
    
    private AudioSource[] _sfxPlayers;
    private Dictionary<string, SoundData> _sfxLookup = new Dictionary<string, SoundData>();
    private int _nextPoolIndex = 0;

    protected override void OnInitialize()
    {
        // SFX 데이터 딕셔너리화
        foreach (var data in sfxDataList)
        {
            if (string.IsNullOrEmpty(data.soundName) || data.clip == null) continue;
            _sfxLookup[data.soundName] = data;
        }

        // SFX 오디오 소스 풀 생성
        _sfxPlayers = new AudioSource[sfxPoolCount];
        for (int i = 0; i < sfxPoolCount; i++)
        {
            _sfxPlayers[i] = gameObject.AddComponent<AudioSource>();
            _sfxPlayers[i].playOnAwake = false;
        }
    }

    /// <summary>
    /// 효과음을 재생합니다. 위치값을 주면 3D 사운드로 재생됩니다.
    /// </summary>
    public void PlaySFX(string soundName, Vector3 position = default)
    {
        if (false == _sfxLookup.TryGetValue(soundName, out var data))
        {
            Debug.LogWarning($"[SoundManager] {soundName} 이름의 사운드를 찾을 수 없습니다.");
            return;
        }

        AudioSource player = _sfxPlayers[_nextPoolIndex];
        
        // 위치값이 있으면 3D, 없으면 2D 사운드 설정
        //if (position != default)
        //{
        //    player.transform.position = position;
        //    player.spatialWeight = 1.0f; // 3D
        //}
        //else
        //{
        //    player.spatialWeight = 0.0f; // 2D
        //}

        player.clip = data.clip;
        player.volume = data.volume;
        player.Play();

        // 다음 풀 인덱스로 이동
        _nextPoolIndex = (_nextPoolIndex + 1) % sfxPoolCount;
    }
}
