using System.Collections.Generic;
using UnityEngine;

namespace _01._Script.Item
{
    public class LootManager : SingletonBase<LootManager>
    {
        private readonly Dictionary<string, List<DropData>> _dropTable = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();
            LoadDropTable();
        }

        private void LoadDropTable()
        {
            var tsvFile = Resources.Load<TextAsset>("Data/DropTable");
            if (tsvFile == null)
            {
                Debug.LogError("LootManager: DropTable 파일을 찾을 수 없습니다. (Resources/Data/DropTable.txt)");
                return;
            }

            var lines = tsvFile.text.Split(new[] { "\r\n", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);

            // 첫 번째 줄(Header)을 제외하고 파싱
            for (var i = 1; i < lines.Length; i++)
            {
                var row = lines[i].Split('\t');
                if (row.Length < 6) continue;

                DropData data = new DropData
                {
                    monsterType = row[0],
                    itemName = row[1],
                    minCount = int.Parse(row[2]),
                    maxCount = int.Parse(row[3]),
                    dropRate = float.Parse(row[4]),
                    prefabPath = row[5].Trim()
                };

                if (_dropTable.ContainsKey(data.monsterType) == false)
                {
                    _dropTable[data.monsterType] = new List<DropData>();
                }

                _dropTable[data.monsterType].Add(data);
            }
        }

        public void DropItems(string monsterType, Vector3 position)
        {
            if (_dropTable.ContainsKey(monsterType) == false)
            {
                Debug.LogWarning($"LootManager: {monsterType} 에 해당하는 드랍 테이블이 없습니다.");
                return;
            }

            foreach (var data in _dropTable[monsterType])
            {
                // 드랍 확률 체크
                if (Random.value > data.dropRate) continue;

                // 드랍 개수 결정
                var count = Random.Range(data.minCount, data.maxCount + 1);
                
                // 프리팹 생성
                var prefab = Resources.Load<GameObject>(data.prefabPath);
                if (prefab != null)
                {
                    // 겹치지 않도록 약간의 랜덤 포지션 추가
                    var randomOffset = new Vector3(Random.Range(-1f, 1f), 0.1f, Random.Range(-1f, 1f));
                    var droppedItem = Instantiate(prefab, position + randomOffset, Quaternion.identity);
                    
                    if (droppedItem.TryGetComponent<ItemObject>(out var itemObj))
                    {
                        if (System.Enum.TryParse(data.itemName, out ItemType type))
                        {
                            itemObj.Initialize(type, count, data.itemName);
                        }
                    }
                }
                else
                {
                    Debug.LogWarning($"LootManager: {data.prefabPath} 경로의 프리팹을 로드할 수 없습니다.");
                }
            }
        }
    }
}
