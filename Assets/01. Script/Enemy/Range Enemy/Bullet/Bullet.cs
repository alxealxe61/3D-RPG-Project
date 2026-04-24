using System.Collections.Generic;
using _01._Script.CombatSystem;
using _01._Script.Enemy.Range_Enemy;
using UnityEngine;

public class Bullet : MonoBehaviour, IHitDetector
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private float lifeTime = 3f;

    [field: SerializeField] private Collider Collider { get; set; }

    private Vector3 moveDirection;
    private bool isLaunched;

    private HashSet<ICombatAgent> hitAgents = new HashSet<ICombatAgent>();

    public ICombatAgent Owner { get; private set; }

    public void Initialize(ICombatAgent owner)
    {
        Owner = owner;
        Collider = GetComponent<Collider>();
    }

    public void EnableDetection()
    {
        Collider.enabled = true;
    }

    public void DisableDetection()
    {
        Collider.enabled = false;
        hitAgents.Clear();
    }

    public void Launch(Vector3 targetPos)
    {
        Vector3 spawnPos = transform.position;
        targetPos.y = spawnPos.y - 1;

        moveDirection = (targetPos - spawnPos).normalized;
        isLaunched = true;
        EnableDetection();
        Invoke(nameof(ReturnToPool), lifeTime);
    }

    void Update()
    {
        if (isLaunched == false) return;
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    private void ReturnToPool()
    {
        Debug.Log("Returning to the pool");
        DisableDetection();
        isLaunched = false;
        CancelInvoke();
        BulletPool.Instance.ReturnToPool(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CombatSystem.Instance.HasHurtBox(other) == false) return;
        if (other.gameObject.layer == this.gameObject.layer) return;
        HurtBox hurtBox = CombatSystem.Instance.GetHurtBox(other);
        ICombatAgent receiver = hurtBox.Owner;
        if (hitAgents.Contains(receiver)) return;
        hitAgents.Add(receiver);

        HitInfo hitInfo = new HitInfo();
        hitInfo.hurtBox = CombatSystem.Instance.GetHurtBox(other);
        hitInfo.receiver = hitInfo.hurtBox.Owner;
        //hitInfo.layerMask = gameObject.layer;
        hitInfo.stun = true;
        ReturnToPool();
        
        Owner.OnHitDetected(hitInfo);
    }
}