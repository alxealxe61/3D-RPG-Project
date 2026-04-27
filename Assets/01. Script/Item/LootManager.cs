using System.Collections.Generic;
using UnityEngine;

namespace _01._Script.Item
{
    public class LootManager : SingletonBase<LootManager>
    {
        private readonly Dictionary<string, List<DropData>> dropTable = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();
            LoadDropTable();
        }

        private void LoadDropTable()
        {
            TextAsset tsvFile = Resources.Load<TextAsset>("Data/DropTable");
            if (tsvFile == null)
            {
                Debug.LogError("LootManager: DropTable 파일을 찾을 수 없습니다. (Resources/Data/DropTable.txt)");
                return;
            }

            string[] lines = tsvFile.text.Split(new[] { "\r\n", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);

            // 첫 번째 줄(Header)을 제외하고 파싱
            for (int i = 1; i < lines.Length; i++)
            {
                string[] row = lines[i].Split('\t');
                if (row.Length < 6) continue;

                DropData data = new DropData
                {
                    MonsterType = row[0],
                    ItemName = row[1],
                    MinCount = int.Parse(row[2]),
                    MaxCount = int.Parse(row[3]),
                    DropRate = float.Parse(row[4]),
                    PrefabPath = row[5].Trim()
                };

                if (dropTable.ContainsKey(data.MonsterType) == false)
                {
                    dropTable[data.MonsterType] = new List<DropData>();
                }

                dropTable[data.MonsterType].Add(data);
            }
        }

        public void DropItems(string monsterType, Vector3 position)
        {
            if (dropTable.ContainsKey(monsterType) == false)
            {
                Debug.LogWarning($"LootManager: {monsterType} 에 해당하는 드랍 테이블이 없습니다.");
                return;
            }

            foreach (var data in dropTable[monsterType])
            {
                // 드랍 확률 체크
                if (Random.value > data.DropRate) continue;

                // 드랍 개수 결정
                int count = Random.Range(data.MinCount, data.MaxCount + 1);
                
                // 프리팹 생성
                GameObject prefab = Resources.Load<GameObject>(data.PrefabPath);
                if (prefab != null)
                {
                    // 겹치지 않도록 약간의 랜덤 포지션 추가
                    Vector3 randomOffset = new Vector3(Random.Range(-0.5f, 0.5f), 0.5f, Random.Range(-0.5f, 0.5f));
                    GameObject droppedItem = Instantiate(prefab, position + randomOffset, Quaternion.identity);
                    
                    if (droppedItem.TryGetComponent<ItemObject>(out var itemObj))
                    {
                        if (System.Enum.TryParse(data.ItemName, out ItemType type))
                        {
                            itemObj.Initialize(type, count, data.ItemName);
                        }
                    }
                }
                else
                {
                    Debug.LogWarning($"LootManager: {data.PrefabPath} 경로의 프리팹을 로드할 수 없습니다.");
                }
            }
        }
    }
}
