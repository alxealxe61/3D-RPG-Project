using System.IO;
using UnityEngine;
using _01._Script;
using _01._Script.Data;

namespace _01._Script
{
    public class DataManager : SingletonBase<DataManager>
    {
        [SerializeField]
        private DataProfile defaultProfile; 

        private DataProfile activeProfile;
        
        /// <summary>
        /// 현재 플레이 세션에서 사용 중인 실시간 프로필입니다.
        /// </summary>
        public DataProfile ActiveProfile
        {
            get
            {
                if (activeProfile == null)
                {
                    activeProfile = Instantiate(defaultProfile);
                }
                return activeProfile;
            }
        }

        // 슬롯 번호에 따른 파일 경로 반환 (Application.persistentDataPath 사용)
        private string GetSavePath(int slotIndex) => Path.Combine(Application.persistentDataPath, $"SaveGame_{slotIndex}.json");

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// 특정 슬롯에 현재 실시간 데이터를 파일로 저장합니다.
        /// </summary>
        public void SaveGame(int slotIndex)
        {
            var saveData = ActiveProfile.GetSaveData();
            string json = JsonUtility.ToJson(saveData, true);
            File.WriteAllText(GetSavePath(slotIndex), json);
            Debug.Log($"[DataManager] Slot {slotIndex} Saved Successfully.");
        }

        /// <summary>
        /// 특정 슬롯의 파일 데이터를 불러와 ActiveProfile에 덮어씌웁니다.
        /// </summary>
        public bool LoadGame(int slotIndex)
        {
            string path = GetSavePath(slotIndex);
            if (!File.Exists(path)) return false;

            try
            {
                string json = File.ReadAllText(path);
                var saveData = JsonUtility.FromJson<DataProfile.SaveData>(json);
                ActiveProfile.LoadFromData(saveData);
                Debug.Log($"[DataManager] Slot {slotIndex} Loaded.");
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[DataManager] Load Failed: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// 슬롯의 데이터를 삭제합니다.
        /// </summary>
        public void DeleteSave(int slotIndex)
        {
            string path = GetSavePath(slotIndex);
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log($"[DataManager] Slot {slotIndex} Deleted.");
            }
        }

        /// <summary>
        /// UI 표시용 데이터 프리뷰
        /// </summary>
        public DataProfile.SaveData GetSaveDataPreview(int slotIndex)
        {
            string path = GetSavePath(slotIndex);
            if (!File.Exists(path)) return null;

            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<DataProfile.SaveData>(json);
        }

        /// <summary>
        /// 새로운 게임 세션을 위해 프로필을 초기화합니다.
        /// </summary>
        public void CreateNewGame()
        {
            activeProfile = Instantiate(defaultProfile);
            Debug.Log("[DataManager] New Game Created.");
        }
    }
}
