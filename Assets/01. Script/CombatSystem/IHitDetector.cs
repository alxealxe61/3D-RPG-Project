using UnityEngine;

namespace _01._Script.CombatSystem
{
    public interface IHitDetector
    {
        ICombatAgent Owner { get; }
        
        
        void Initialize(ICombatAgent owner);
        void EnableDetection();
        void DisableDetection();
    }
}