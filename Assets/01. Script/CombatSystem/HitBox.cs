using System;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    [Header("피격자 정보")] 
    public LayerMask myLayerMask;

    public GameObject owner; // 이 무기의 주인 (공격자)

    public Collider collider;

    [HideInInspector] public int myLayerNumber;
    
    private void Awake() 
    {
        // 인스펙터에서 선택한 LayerMask를 실제 레이어 번호(int)로 변환
        if (myLayerMask.value > 0)
        {
            myLayerNumber = (int)Mathf.Log(myLayerMask.value, 2);
        }
        
        collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true;
            collider.enabled = false;
        }
    }

    

    public void AttackStart()
    {
        collider.enabled = true;
    }

    public void AttackStop()
    {
        collider.enabled = false;
    }
    
    private void OnTriggerEnter(Collider other) 
    {
        // 상대방에게 HurtComponent가 있는지 확인
        if (other.TryGetComponent<HurtBox>(out var hurt))
        {
            // 💡 LayerMask에서 레이어 번호(int) 추출
            int myLayerNumber = (int)Mathf.Log(myLayerMask.value, 2);

            // 상대방 레이어와 내 레이어가 같으면(아군) 무시
            if (hurt.myLayerNumber == myLayerNumber) return;
            
            HitInfo hitData = new HitInfo(owner.GetInstanceID(), other.gameObject.GetInstanceID(), myLayerNumber);
            
            //Debug.Log("Hit: 내가 대상을 때렸다고 알림");
            CombatSystem.Instance.RegisterHit(hitData);
        }
    }
}
