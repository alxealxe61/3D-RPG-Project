using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyProfile", menuName = "GameData/EnemyProfile")]
public class EnemyProfile : ScriptableObject
{
    [Header("--- Battle Stats ---")]
    public int MaxHp;
    public int MaxAttack;
    public int moveSpeed = 5;
}