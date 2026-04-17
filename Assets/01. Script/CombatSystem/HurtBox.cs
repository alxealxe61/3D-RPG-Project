using System;
using _01._Script.CombatSystem;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HurtBox : MonoBehaviour, IHitTargetPart
{
    public ICombatAgent Owner { get; set; }
    
    public Collider Collider { get; private set; }

    private void Awake()
    {
        Collider = GetComponent<Collider>();
    }
    
    public void Initialize(ICombatAgent owner)
    {
        Owner = owner;
        CombatSystem.Instance.AddHurtBox(Collider, this);
    }
    
    private void OnDestroy()
    {
        CombatSystem.Instance.RemoveHurtBox(Collider, this);
    }
}