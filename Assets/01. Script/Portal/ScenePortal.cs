using UnityEngine;

namespace _01._Script.Portal
{
    public class ScenePortal : MonoBehaviour
    {
        [Header("Portal Settings")]

        [Header("Local Portal Info")]
        [Tooltip("이 포탈의 고유 ID (다른 씬에서 이 씬으로 올 때 참조됨)")]
        public string currentPortalID;

        [Header("Target Destination")]
        [SerializeField] private string targetSceneName;
        [SerializeField] private string targetPortalID;

        [Header("Spawn Settings")]
        [SerializeField] private Transform spawnPoint;
        public Transform SpawnPoint => spawnPoint != null ? spawnPoint : transform;

        public void Interact()
        {
            if (string.IsNullOrEmpty(targetSceneName))
            {
                Debug.LogWarning($"[ScenePortal] {gameObject.name}의 targetSceneName이 설정되지 않았습니다.");
                return;
            }

            Debug.Log($"[ScenePortal] {targetSceneName} 씬으로 이동을 요청합니다. (도착 포탈: {targetPortalID})");
            SceneTransitionManager.Instance.TransitionToScene(targetSceneName, targetPortalID);
        }

        private void OnDrawGizmos()
        {
            // 기즈모를 통해 스폰 위치와 전방 방향 시각화
            Gizmos.color = Color.cyan;
            var pos = SpawnPoint.position;
            Gizmos.DrawSphere(pos, 0.3f);
            Gizmos.DrawLine(pos, pos + SpawnPoint.forward * 1.0f);
        }
    }
}
