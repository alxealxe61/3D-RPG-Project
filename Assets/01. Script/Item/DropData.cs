using UnityEngine.Serialization;

namespace _01._Script.Item
{
    [System.Serializable]
    public class DropData
    {
        public string monsterType;
        public string itemName;
        public int minCount;
        public int maxCount;
        public float dropRate;
        public string prefabPath;
    }
}
