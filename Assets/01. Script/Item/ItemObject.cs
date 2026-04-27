using UnityEngine;

namespace _01._Script.Item
{
    public class ItemObject : MonoBehaviour
    {
        public ItemType itemType;
        public int count;
        public string itemName;

        public void Initialize(ItemType type, int amount, string name)
        {
            itemType = type;
            count = amount;
            itemName = name;
        }

        public void OnCollected()
        {
            // TODO: 획득 사운드나 파티클 효과가 필요하면 여기에 추가하세요.
            Destroy(gameObject);
        }
    }
}
