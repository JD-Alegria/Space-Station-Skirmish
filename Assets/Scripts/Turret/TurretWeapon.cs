using UnityEngine;
using System.Collections.Generic;

public class TurretWeapon : MonoBehaviour
{
    FireSupportType weaponType;
    float range = 100f;
    float fireRate = 1f;
    float damage = 10f;
    float ionProjectileLifetime = 0.5f;
    float nextFireTime;
    [SerializeField] Transform muzzle;
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
    

    public void Init(FireSupportPlatformData data)
    {
        weaponType = data.FireSupportType;
        range = data.AttackRange;
        fireRate = data.AttackFireRateLevel1;
        damage = data.AttackDamageLevel1;
        bulletPrefab = data.BulletPrefab;
        ionProjectilePrefab = data.IonProjectilePrefab;
        impactFxPrefab = data.ImpactFxPrefab;
        bulletSFX = data.BulletSFX;
        ionProjectileSFX = data.IonProjectileSFX;
        impactSFX = data.ImpactSFX;
        ionProjectileLifetime = data.IonProjectileLifetime;
    }

    public void UpgradeValues(FireSupportPlatformData data)
    {
        range = data.AttackRange;
        fireRate = data.AttackFireRateLevel2;
        damage = data.AttackDamageLevel2;
    }

    public void Fire()
    {
        if (Time.time < nextFireTime) return;
        
        nextFireTime = Time.time + 1f / fireRate;
        FireHitScan();
    }

    void FireHitScan()
    {
        if (muzzle == null) return;
        
        Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit hit, range, hitMask);
        
        //debug tracer code
        
        FireVFX(weaponType, hit);

        if (hit.collider == null) return;

        if (hit.collider.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.ApplyDamage(new DamageInfo(damage, hit.collider.gameObject, hit.point));
        }
    }

    void FireVFX(FireSupportType weaponType, RaycastHit hit)
    {
        bool hitSomething = hit.collider != null;
        Vector3 startPos = muzzle.position;
        Vector3 endPos = hitSomething ? hit.point : startPos + muzzle.forward * range;
        
        if (debugTracerPrefab != null)
        {
            Instantiate(debugTracerPrefab).GetComponent<BulletTracer>().Init(startPos, endPos, true);
            return;
        }
        
        //instantiate bullet prefab based on weaponType
        switch (weaponType)
        {
            case FireSupportType.GunTurret:
                if (bulletPrefab == null) break;
                Instantiate(bulletPrefab).GetComponent<BulletTracer>().Init(startPos, endPos, false);
                break;
            case FireSupportType.IonTurret:
                Instantiate(ionProjectilePrefab).GetComponent<IonProjectile>().Init(startPos, endPos, ionProjectileLifetime);
                break;
        }
    }
}
