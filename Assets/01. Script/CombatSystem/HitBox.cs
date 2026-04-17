using System.Collections.Generic;
using _01._Script.CombatSystem;
using UnityEngine;

public class HitBox : MonoBehaviour, IHitDetector
{
    public ICombatAgent Owner { get; private set; }
    
    [field: SerializeField]  private Collider Collider { get; set; }
    
    private HashSet<ICombatAgent> hitAgents = new HashSet<ICombatAgent>();
    
    public void Initialize(ICombatAgent owner)
    {
        Owner = owner;
        Collider =  GetComponent<Collider>();
    }

    [ContextMenu("Start Attack")]
    public void EnableDetection()
    {
        Collider.enabled = true;
    }

    [ContextMenu("Stop Attack")]
    public void DisableDetection()
    {
        Collider.enabled = false;
        hitAgents.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CombatSystem.Instance.HasHurtBox(other) == false) return;
        
        HurtBox hurtBox = CombatSystem.Instance.GetHurtBox(other);
        ICombatAgent receiver = hurtBox.Owner;
        if (hitAgents.Contains(receiver)) return;
        hitAgents.Add(receiver);
        
        HitInfo hitInfo = new HitInfo();
        hitInfo.hurtBox = CombatSystem.Instance.GetHurtBox(other);
        hitInfo.receiver = hitInfo.hurtBox.Owner;
        hitInfo.layerMask = gameObject.layer;
        
        Owner.OnHitDetected(hitInfo);
    }
}
