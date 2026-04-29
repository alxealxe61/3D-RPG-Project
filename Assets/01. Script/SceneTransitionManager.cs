using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using _01._Script.Environment;

public class SceneTransitionManager : SingletonBase<SceneTransitionManager>
{
    private string _targetPortalID;
    private bool _isTransitioning = false;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    protected override void OnDispose()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        base.OnDispose();
    }

    /// <summary>
    /// 씬 전환을 시작합니다.
    /// </summary>
    /// <param name="sceneName">목적지 씬 이름</param>
    /// <param name="targetPortalID">목적지 씬에서 도착할 포탈의 ID</param>
    public void TransitionToScene(string sceneName, string targetPortalID)
    {
        if (_isTransitioning) return;
        
        _targetPortalID = targetPortalID;
        _isTransitioning = true;
        
        // 씬 로드 (필요시 페이드 인/아웃 추가 가능)
        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!_isTransitioning) return;

        StartCoroutine(SetupPlayerPosition());
    }

    private IEnumerator SetupPlayerPosition()
    {
        // 씬 로드 후 한 프레임 대기하여 모든 오브젝트가 Initialize 되도록 함
        yield return null;

        if (string.IsNullOrEmpty(_targetPortalID))
        {
            _isTransitioning = false;
            yield break;
        }

        // 새 씬에서 목적지 포탈 찾기
        ScenePortal[] portals = Object.FindObjectsByType<ScenePortal>(FindObjectsSortMode.None);
        ScenePortal targetPortal = null;

        foreach (var portal in portals)
        {
            if (portal.currentPortalID == _targetPortalID)
            {
                targetPortal = portal;
                break;
            }
        }

        if (targetPortal != null)
        {
            TeleportPlayer(targetPortal.SpawnPoint);
        }
        else
        {
            Debug.LogWarning($"[SceneTransitionManager] 포탈 ID '{_targetPortalID}'를 가진 포탈을 찾을 수 없습니다.");
        }

        _targetPortalID = null;
        _isTransitioning = false;
    }

    private void TeleportPlayer(Transform spawnPoint)
    {
        // PlayerController 싱글톤 인스턴스 사용
        PlayerController player = PlayerController.Instance;
        
        if (player == null)
        {
            Debug.LogWarning("[SceneTransitionManager] PlayerController.Instance가 존재하지 않습니다.");
            return;
        }

        // Rigidbody가 있다면 속도 초기화 (물리 버그 방지)
        if (player.rb != null)
        {
            player.rb.linearVelocity = Vector3.zero;
            player.rb.angularVelocity = Vector3.zero;
        }

        // 위치 및 회전 설정
        player.transform.position = spawnPoint.position;
        player.transform.rotation = spawnPoint.rotation;
        
        Debug.Log($"[SceneTransitionManager] 플레이어를 {spawnPoint.name} 위치로 이동시켰습니다.");
    }
}
