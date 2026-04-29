using UnityEngine;

namespace _01._Script.Data
{
    [CreateAssetMenu(fileName = "NewPlayerProfile", menuName = "GameData/PlayerProfile")]
    public class DataProfile : ScriptableObject
    {
        [Header("--- Battle Stats ---")]
        public int MaxHp;
        public int MaxAttack;
        public int maxSkillPoint = 10;
        public int moveSpeed = 5;

        [Header("--- Wallet & Inventory ---")]
        public int gold;          // 현재 보유 골드
        public int upgradeStones; // 보유 중인 강화석 개수

        [Header("--- Progress ---")]
        public int weaponLevel;   // 현재 무기 강화 단계

        /// <summary>
        /// 데이터를 파일에 저장하기 위한 단순한 구조체입니다.
        /// </summary>
        [System.Serializable]
        public class SaveData
        {
            public int maxHp;
            public int maxAttack;
            public int maxSkillPoint;
            public int moveSpeed;
            public int gold;
            public int upgradeStones;
            public int weaponLevel;
        }

        /// <summary>
        /// 현재 SO의 데이터를 SaveData 객체로 추출합니다.
        /// </summary>
        public SaveData GetSaveData()
        {
            return new SaveData
            {
                maxHp = this.MaxHp,
                maxAttack = this.MaxAttack,
                maxSkillPoint = this.maxSkillPoint,
                moveSpeed = this.moveSpeed,
                gold = this.gold,
                upgradeStones = this.upgradeStones,
                weaponLevel = this.weaponLevel
            };
        }

        /// <summary>
        /// 로드된 SaveData를 현재 SO에 적용합니다.
        /// </summary>
        public void LoadFromData(SaveData data)
        {
            this.MaxHp = data.maxHp;
            this.MaxAttack = data.maxAttack;
            this.maxSkillPoint = data.maxSkillPoint;
            this.moveSpeed = data.moveSpeed;
            this.gold = data.gold;
            this.upgradeStones = data.upgradeStones;
            this.weaponLevel = data.weaponLevel;
        }
    }
}