using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatSystem : MonoBehaviour
{
    public static CombatSystem Instance;

    private List<HitInfo> hitRequests = new List<HitInfo>();
    private List<HitInfo> hurtRequests = new List<HitInfo>();
    
    [Header("설정")]
    [SerializeField] private float validationWindow = 0.1f; // 판정 유효 시간

    private void Awake() 
    {
        Instance = this;
    }

    // 공격자로부터 받은 신호
    public void RegisterHit(HitInfo info) 
    {
        // Hurt 리스트에서 나(공격자)와 상대(피격자)가 일치하는 신호가 있는지 확인
        var match = hurtRequests.Find(h => h.attackerId == info.attackerId && h.victimId == info.victimId);
        
        if (match.attackerId != 0) 
        {
            ProcessFinalHit(info);
            hurtRequests.Remove(match);
        } 
        else 
        {
            hitRequests.Add(info);
            StartCoroutine(CleanUpRequest(info, true));
        }
    }

    // 피격자로부터 받은 신호
    public void RegisterHurt(HitInfo info) 
    {
        // Hit 리스트에서 나(피격자)와 상대(공격자)가 일치하는 신호가 있는지 확인
        var match = hitRequests.Find(h => h.attackerId == info.attackerId && h.victimId == info.victimId);

        if (match.attackerId != 0) 
        {
            ProcessFinalHit(info);
            hitRequests.Remove(match);
        } 
        else 
        {
            hurtRequests.Add(info);
            StartCoroutine(CleanUpRequest(info, false));
        }
    }

    // 실제 데미지 로직이 실행되는 곳
    private void ProcessFinalHit(HitInfo info) 
    {
        Debug.Log($"<color=green><b>[판정 확정]</b></color> {info.attackerId} -> {info.victimId} 타격 성공!");
        // 여기서 실제로 체력을 깎거나 이펙트를 생성합니다.
    }

    private IEnumerator CleanUpRequest(HitInfo info, bool isHitList)
    {
        yield return new WaitForSeconds(validationWindow);
        if (isHitList) hitRequests.Remove(info);
        else hurtRequests.Remove(info);
    }
}