using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewPlayerProfile", menuName = "GameData/PlayerProfile")]
public class PlayerProfile : ScriptableObject
{
    [Header("--- Battle Stats ---")]
    public int MaxHp;
    public int MaxAttack;
    public int maxSkillGauge = 10;
    public int moveSpeed = 5;
    
    [Header("--- Wallet & Inventory ---")]
    public int gold;          // 현재 보유 골드
    public int upgradeStones; // 보유 중인 강화석 개수

    [Header("--- Progress ---")]
    public int weaponLevel;   // 현재 무기 강화 단계
}