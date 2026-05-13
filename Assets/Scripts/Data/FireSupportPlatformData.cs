using UnityEngine;

public enum FireSupportType
{
    GunTurret,
    IonTurret
}

[CreateAssetMenu(fileName = "FireSupportPlatformData", menuName = "Scriptable Objects/FireSupportPlatformData")]
public class FireSupportPlatformData : PlatformData
{
    [Header("Fire Support Platform Info")]
    [SerializeField] FireSupportType fireSupportType;
    
    public FireSupportType FireSupportType => fireSupportType;

    [Header("Fire Support Platform Stats")] 
    [SerializeField] float detectionRadius;
    [SerializeField] float attackRange;
    [SerializeField] float attackDamageLevel1;
    [SerializeField] float attackDamageLevel2;
    [SerializeField] float attackFireRateLevel1;
    [SerializeField] float attackFireRateLevel2;
    
    [Header("Fire Support Platform FX")]
    [Space]
    [Header("VFX")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject ionProjectilePrefab;
    [SerializeField] GameObject impactFxPrefab;
    [SerializeField] float ionProjectileLifetime;
    
    public float IonProjectileLifetime => ionProjectileLifetime;

    [Header("SFX")] 
    [SerializeField] AudioClip bulletSFX;
    [SerializeField] AudioClip ionProjectileSFX;
    [SerializeField] AudioClip impactSFX;

    public float DetectionRadius => detectionRadius;
    public float AttackRange => attackRange;
    public float AttackDamageLevel1 => attackDamageLevel1;
    public float AttackDamageLevel2 => attackDamageLevel2;
    public float AttackFireRateLevel1 => attackFireRateLevel1;
    public float AttackFireRateLevel2 => attackFireRateLevel2;
    public GameObject BulletPrefab => bulletPrefab;
    public GameObject IonProjectilePrefab => ionProjectilePrefab;
    public GameObject ImpactFxPrefab => impactFxPrefab;
    public AudioClip BulletSFX => bulletSFX;
    public AudioClip IonProjectileSFX => ionProjectileSFX;
    public AudioClip ImpactSFX => impactSFX;
}
