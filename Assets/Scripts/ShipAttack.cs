using UnityEngine;

public class ShipAttack : MonoBehaviour
{
    float range = 100f;
    float fireRate = 1f;
    float damage = 10f;
    float nextFireTime;
    [SerializeField] Transform muzzle1;
    [SerializeField] Transform muzzle2;
    [SerializeField] LayerMask hitMask;
    [SerializeField] GameObject debugTracerPrefab;
    
    [Header("VFX")]
    GameObject bulletPrefab;
    GameObject ionProjectilePrefab;
    GameObject impactFxPrefab;

    [Header("SFX")] 
    AudioClip bulletSFX;
    AudioClip ionProjectileSFX;
    AudioClip impactSFX;

    public void Init(EnemyShipData data)
    {
        range = data.AttackRange;
        fireRate = data.AttackRate;
        damage = data.AttackDamage;
        
        bulletSFX = data.BulletSFX;
        ionProjectileSFX = data.IonProjectileSFX;
        impactSFX = data.ImpactSFX;
    }

    public void Fire()
    {
        if (Time.time < nextFireTime) return;
        
        nextFireTime = Time.time + 1f / fireRate;
        FireHitScan();
    }

    void FireHitScan()
    {
        if (muzzle1 == null && muzzle2 == null) return;
        
        Physics.Raycast(muzzle1.position, muzzle1.forward, out RaycastHit hit, range, hitMask);
        
        //debug tracer code
        bool hitSomething = false;
        Vector3 startPos = muzzle1.position;
        Vector3 endPos = hitSomething ? hit.point : startPos + muzzle1.forward * range;

        Instantiate(debugTracerPrefab).GetComponent<BulletTracer>().Init(startPos, endPos, true);
        //debug tracer code

        if (hit.collider == null) return;

        if (hit.collider.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.ApplyDamage(new DamageInfo(damage, hit.collider.gameObject, hit.point));
        }
        
        Physics.Raycast(muzzle2.position, muzzle2.forward, out hit, range, hitMask);
        
        //debug tracer code
        hitSomething = false;
        startPos = muzzle2.position;
        endPos = hitSomething ? hit.point : startPos + muzzle2.forward * range;

        Instantiate(debugTracerPrefab).GetComponent<BulletTracer>().Init(startPos, endPos, true);
        //debug tracer code

        if (hit.collider == null) return;

        if (hit.collider.TryGetComponent<IDamageable>(out damageable))
        {
            damageable.ApplyDamage(new DamageInfo(damage, hit.collider.gameObject, hit.point));
        }
    }

}
