using UnityEngine;

namespace _01._Script.CombatSystem
{
    public interface IHitTargetPart
    {
        ICombatAgent Owner { get; }
        Collider Collider { get; }
        void Initialize(ICombatAgent owner);
    }
}