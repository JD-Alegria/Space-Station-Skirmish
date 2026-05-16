using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ShipAttack : MonoBehaviour
{
    AudioSource audioSource;
    
    float range = 100f;
    float fireRate = 1f;
    float damage = 10f;
    float nextFireTime;
    [SerializeField] Transform muzzle1;
    [SerializeField] Transform muzzle2;
    [SerializeField] LayerMask hitMask;
    [SerializeField] GameObject debugTracerPrefab;

    GameObject muzzleEffect1;
    GameObject muzzleEffect2;
    
    [Header("VFX")]
    GameObject bulletPrefab;
    GameObject muzzleEffectPrefab;
    GameObject impactFxPrefab;

    [Header("SFX")] 
    AudioClip bulletSFX;
    AudioClip impactSFX;

    Coroutine muzzleEffect1Coroutine;
    Coroutine muzzleEffect2Coroutine;

    public void Init(EnemyShipData data)
    {
        range = data.AttackRange;
        fireRate = data.AttackRate;
        damage = data.AttackDamage;
        
        bulletPrefab = data.BulletPrefab;
        bulletSFX = data.BulletSFX;
        muzzleEffectPrefab = data.MuzzleEffectPrefab;
        impactSFX = data.ImpactSFX;

        muzzleEffect1 = Instantiate(muzzleEffectPrefab, muzzle1);
        muzzleEffect2 = Instantiate(muzzleEffectPrefab, muzzle2);
        
        audioSource = GetComponent<AudioSource>();
    }

    public void Fire()
    {
        if (Time.time < nextFireTime) return;
        
        nextFireTime = Time.time + 1f / fireRate;
        FireHitScan();
    }

    void FireHitScan()
    {
        if (muzzle1 == null || muzzle2 == null) return;
        
        Physics.Raycast(muzzle1.position, muzzle1.forward, out RaycastHit hit, range, hitMask);
        
        //debug tracer code
        bool hitSomething = hit.collider != null;
        Vector3 startPos = muzzle1.position;
        Vector3 endPos = hitSomething ? hit.point : startPos + muzzle1.forward * range;

        // instantiate bullet
        Instantiate(bulletPrefab).GetComponent<BulletTracer>().Init(startPos, endPos, false);
        
        //play sfx
        PlayMuzzleEffects();
        
        if (hit.collider == null) return;

        if (hit.collider.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.ApplyDamage(new DamageInfo(damage, gameObject, hit.point));
        }
        
        Physics.Raycast(muzzle2.position, muzzle2.forward, out hit, range, hitMask);
        
        hitSomething = hit.collider != null;
        startPos = muzzle2.position;
        endPos = hitSomething ? hit.point : startPos + muzzle2.forward * range;

        Instantiate(bulletPrefab).GetComponent<BulletTracer>().Init(startPos, endPos, false);

        if (hit.collider == null) return;

        if (hit.collider.TryGetComponent<IDamageable>(out damageable))
        {
            damageable.ApplyDamage(new DamageInfo(damage, gameObject, hit.point));
        }
    }

    void PlayMuzzleEffects()
    {
        if (muzzleEffect1Coroutine != null)
        {
            StopCoroutine(muzzleEffect1Coroutine);
            muzzleEffect1Coroutine = null;
        }

        if (muzzleEffect2Coroutine != null)
        {
            StopCoroutine(muzzleEffect2Coroutine);
            muzzleEffect2Coroutine = null;
        }
        muzzleEffect1Coroutine = StartCoroutine(MuzzleEffectRoutine(muzzleEffect1));
        muzzleEffect2Coroutine = StartCoroutine(MuzzleEffectRoutine(muzzleEffect2));
        
        audioSource.PlayOneShot(bulletSFX, .33f);
    }

    IEnumerator MuzzleEffectRoutine(GameObject muzzle)
    {
        WaitForSeconds wait = new WaitForSeconds(0.125f);
        muzzle.SetActive(true);
        yield return wait;
        muzzle.SetActive(false);
    }
}
