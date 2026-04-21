using UnityEngine;
using System.Collections.Generic;

// 이 스크립트가 들어간 오브젝트에는 반드시 트리거로 설정된 Collider가 있어야 합니다.
[RequireComponent(typeof(SphereCollider))]
public class LockOnSystem : MonoBehaviour
{
    [Header("Tags")]
    public string enemyTag = "Enemy";   // 일반 몬스터 태그
    public string bossTag = "Boss";     // 보스 몬스터 태그

    [Header("Camera & Rotation")]
    public Transform cameraTransform;
    public float rotationSpeed = 10f;

    // 감지된 적들을 담아둘 List
    private List<Transform> targetList = new List<Transform>();
    
    // 현재 상태 변수
    public Transform CurrentTarget => currentTarget;
    public bool IsLockedOn => isLockedOn;

    private Transform currentTarget;
    private int currentTargetIndex = 0;
    private bool isLockedOn = false;

    void Start()
    {
        // 시작 시 자동으로 콜라이더를 Trigger 모드로 변경 (안전 장치)
        Collider col = GetComponent<Collider>();
        if (col != null) col.isTrigger = true;
    }

    void Update()
    {
        // 1. 죽거나 파괴된 몬스터를 List에서 안전하게 제거
        CleanUpDeadTargets();

        // 2. 록온 상태일 때의 동작
        if (isLockedOn)
        {
            // 타겟이 죽었거나 없어진 경우의 처리
            if (currentTarget == null)
            {
                if (targetList.Count > 0)
                {
                    // 남은 몬스터가 있다면 리스트의 첫 번째(0번) 녀석으로 다시 록온
                    SetTarget(0);
                }
                else
                {
                    // 주변에 몬스터가 한 마리도 없다면 록온 종료
                    ReleaseLockOn();
                    return;
                }
            }
            else
            {
                // 3. 마우스 휠 클릭(2) 시 타겟 변경
                if (Input.GetMouseButtonDown(2))
                {
                    SwitchTarget();
                }
            }
        }
    }

    // 트리거(감지 범위) 안에 들어왔을 때
    private void OnTriggerEnter(Collider other)
    {
        // 들어온 대상이 적이거나 보스라면
        if (other.CompareTag(enemyTag) || other.CompareTag(bossTag))
        {
            if (!targetList.Contains(other.transform))
            {
                targetList.Add(other.transform);
                //Debug.Log($"List 추가됨: {other.name} / 현재 총 {targetList.Count}마리");

                // 조건: List의 처음으로 들어온 몬스터를 자동으로 록온
                if (!isLockedOn && targetList.Count == 1)
                {
                    SetTarget(0);
                    isLockedOn = true;
                }
            }
        }
    }

    // 트리거(감지 범위) 밖으로 나갔을 때
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(enemyTag) || other.CompareTag(bossTag))
        {
            if (targetList.Contains(other.transform))
            {
                targetList.Remove(other.transform);
                //Debug.Log($"List 제거됨: {other.name} / 남은 수: {targetList.Count}마리");

                // 방금 범위를 벗어난 녀석이 '현재 록온된 타겟'이었다면?
                if (isLockedOn && currentTarget == other.transform)
                {
                    if (targetList.Count > 0)
                    {
                        // 남은 적 중 첫 번째로 타겟 이동
                        SetTarget(0); 
                    }
                    else
                    {
                        // 남은 적이 없으면 시스템 종료
                        ReleaseLockOn(); 
                    }
                }
            }
        }
    }

    // 마우스 휠로 타겟 전환 로직
    private void SwitchTarget()
    {
        // 방어 코드: 현재 타겟이 없거나 1마리 뿐이면 바꿀 필요 없음
        if (currentTarget == null || targetList.Count <= 1) return;

        // 조건: 보스 몬스터면 타겟 변경 금지
        if (currentTarget.CompareTag(bossTag))
        {
            Debug.Log("보스 몬스터에게 록온 고정 중입니다. (변경 불가)");
            return;
        }

        // 인덱스를 1 증가시키고, 범위를 넘어가면 다시 0(처음)으로 순환
        currentTargetIndex = (currentTargetIndex + 1) % targetList.Count;
        SetTarget(currentTargetIndex);
    }

    // 타겟을 지정하는 공통 함수
    private void SetTarget(int index)
    {
        currentTargetIndex = index;
        currentTarget = targetList[currentTargetIndex];
        Debug.Log("현재 타겟: " + currentTarget.name);
    }

    // 록온 시스템 종료
    private void ReleaseLockOn()
    {
        isLockedOn = false;
        currentTarget = null;
        currentTargetIndex = 0;
        Debug.Log("주변에 몬스터가 없어 록온 시스템을 종료합니다.");
    }

    // 죽은 몬스터 리스트 정리 (Missing Reference 방지)
    private void CleanUpDeadTargets()
    {
        // 콜라이더 트리거가 작동하기 전, Destroy로 적이 죽어버리면 null이 리스트에 남습니다.
        // 이를 방지하기 위해 매 프레임 죽은 오브젝트(null)를 리스트에서 제거합니다.
        targetList.RemoveAll(target => target == null || !target.gameObject.activeInHierarchy);
    }
}     