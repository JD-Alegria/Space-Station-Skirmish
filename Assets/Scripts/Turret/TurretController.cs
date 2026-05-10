using FORGE3D;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TurretTracking),  typeof(DetectTargets), typeof(TurretWeapon))]
public class TurretController : MonoBehaviour
{
    TurretTracking turretTracking;
    DetectTargets detectTargets;
    TurretWeapon turretWeapon;
    BatteryController batteryController;

    [SerializeField] FireSupportPlatformData data;
    [SerializeField] float updateInterval = 0.1f;

    Coroutine rotateTowardsTarget;
    Coroutine commandFiring;

    void Awake()
    {
        turretTracking = GetComponent<TurretTracking>();
        detectTargets = GetComponent<DetectTargets>();
        turretWeapon = GetComponent<TurretWeapon>();
    }

    void Start()
    {
        turretWeapon.Init(data);
        detectTargets.Init(data);
        batteryController = GetComponentInParent<BatteryController>();
        
        if (rotateTowardsTarget == null) rotateTowardsTarget = StartCoroutine(RotateToTarget());
        if (commandFiring == null) commandFiring = StartCoroutine(CommandFiring());
    }

    IEnumerator CommandFiring()
    {
        WaitForSeconds wait = new WaitForSeconds(updateInterval);

        while (true)
        {
            yield return wait;
            
            if (!batteryController.IsPowered) continue;
            if (detectTargets.ClosestDamageable == null) continue;
            if (detectTargets.ClosestDamageableDist > data.AttackRange) continue;
            
            turretWeapon.Fire();
        }
    }

    IEnumerator RotateToTarget()
    {
        WaitForSeconds wait = new WaitForSeconds(updateInterval);

        while (true)
        {
            yield return wait;

            if (!batteryController.IsPowered) continue;
            if (IsIDamgeableDestroyed(detectTargets.ClosestDamageable)) continue;
            
            if (detectTargets.ClosestDamageable != null)
            {
                turretTracking.SetNewTarget(detectTargets.ClosestDamageable.GetGameObject().transform.position);
            }
        }
    }
    
    bool IsIDamgeableDestroyed(IDamageable damageable)
    {
        if (damageable is not Object unityObject || unityObject == null) return true;
        return false;
    }
}
