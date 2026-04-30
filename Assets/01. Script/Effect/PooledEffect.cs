using System.Collections;
using UnityEngine;

namespace _01._Script.Effect
{
    public class PooledEffect : MonoBehaviour
    {
        private ParticleSystem[] _particles;

        private void Awake() => _particles = GetComponentsInChildren<ParticleSystem>();

        private void OnEnable()
        {
            var maxDuration = 0f;
            foreach (var ps in _particles)
            {
                var main = ps.main;
                // 강제로 루핑을 꺼서 중복 재생을 방지함
                main.loop = false;
            
                var duration = (main.duration + main.startLifetime.constantMax) / main.simulationSpeed;
                if (duration > maxDuration) maxDuration = duration;
            
                ps.Clear();
                ps.Play();
            }
        
            // 지속 시간이 끝나면 매니저에게 정식 반환 요청
            StartCoroutine(ReturnAfterDelay(maxDuration));
        }
        
        private IEnumerator ReturnAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
        
            // 정식 풀 수거 로직 호출
            if (EffectManager.IsInitialized)
            {
                EffectManager.Instance.ReturnToPool(gameObject);
            }
            else
            {
                gameObject.SetActive(false); 
            }
        }
    }
}
