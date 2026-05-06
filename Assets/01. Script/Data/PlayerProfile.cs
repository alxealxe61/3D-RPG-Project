using UnityEngine;
using UnityEngine.Serialization;

namespace _01._Script.Data
{
    [CreateAssetMenu(fileName = "NewPlayerProfile", menuName = "GameData/PlayerProfile")]
    public class DataProfile : ScriptableObject
    {
        [Header("--- Battle Stats ---")]
        public int maxHp;
        public int maxAttack;
        public int maxSkillPoint = 10;
        public int moveSpeed = 5;

        [Header("--- Wallet & Inventory ---")]
        public int gold;          // 현재 보유 골드
        public int upgradeStones; // 보유 중인 강화석 개수

        [Header("--- Progress ---")]
        public int weaponLevel;   // 현재 무기 강화 단계
        
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
        
        public SaveData GetSaveData()
        {
            return new SaveData
            {
                maxHp = this.maxHp,
                maxAttack = this.maxAttack,
                maxSkillPoint = this.maxSkillPoint,
                moveSpeed = this.moveSpeed,
                gold = this.gold,
                upgradeStones = this.upgradeStones,
                weaponLevel = this.weaponLevel
            };
        }
        
        public void LoadFromData(SaveData data)
        {
            this.maxHp = data.maxHp;
            this.maxAttack = data.maxAttack;
            this.maxSkillPoint = data.maxSkillPoint;
            this.moveSpeed = data.moveSpeed;
            this.gold = data.gold;
            this.upgradeStones = data.upgradeStones;
            this.weaponLevel = data.weaponLevel;
        }
    }
}