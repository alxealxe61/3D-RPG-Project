using UnityEngine;

public struct HitInfo
{
    public int attackerId;    // 공격자 고유 번호
    public int victimId;      // 피격자 고유 번호
    public int attacklayer;  // 공격 타입 (필요 시)

    public HitInfo(int attacker, int victim, int layer)
    {
        attackerId = attacker;
        victimId = victim;
        attacklayer = layer;
    }
}