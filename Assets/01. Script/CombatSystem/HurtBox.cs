using UnityEngine;

public class HurtBox : MonoBehaviour
{
    [Header("피격자 정보")] 
    public LayerMask myLayerMask;

    [HideInInspector] public int myLayerNumber;

    private void Awake() 
    {
        // 인스펙터에서 선택한 LayerMask를 실제 레이어 번호(int)로 변환
        if (myLayerMask.value > 0)
        {
            myLayerNumber = (int)Mathf.Log(myLayerMask.value, 2);
        }
    }
    
    private void OnTriggerEnter(Collider other) 
    {
        // 나를 친 물체에 HitComponent가 있는지 확인
        if (other.TryGetComponent<HitBox>(out var hit))
        {
            int myLayerNumber = (int)Mathf.Log(myLayerMask.value, 2);
            if (hit.myLayerNumber == myLayerNumber) return;
            HitInfo hurtData = new HitInfo(hit.owner.GetInstanceID(), this.gameObject.GetInstanceID(), myLayerNumber);
            
            //Debug.Log("Hurt: 내가 누군가에게 맞았다고 알림");
            CombatSystem.Instance.RegisterHurt(hurtData);
        }
    }
}