using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class DetectTargets : MonoBehaviour
{
    PlatformData data;

    [SerializeField] bool isDetectingTargets;
    [SerializeField] LayerMask targetMask;
    [SerializeField] float updateInterval = 0.1f;
    [SerializeField] float detectionRadius = 10f;
    float closestDamageableDist;
    IDamageable closestDamageable;
    
    public IDamageable ClosestDamageable => closestDamageable;
    public float ClosestDamageableDist => closestDamageableDist;

    Coroutine detectionRoutine;

    public void Init(FireSupportPlatformData data)
    {
        this.data = data;
        detectionRadius = data.DetectionRadius;
    }

    void Start()
    {
        StartTargetDetection();
    }

    public void StartTargetDetection()
    {
        if (detectionRoutine != null) return;
        
        closestDamageable = null;
        isDetectingTargets = true;
        detectionRoutine = StartCoroutine(DetectClosestTargetRoutine());
    }

    public void EndStartDetection()
    {
        if (detectionRoutine == null) return;
        
        StopCoroutine(detectionRoutine);
        detectionRoutine = null;
        closestDamageable = null;
        isDetectingTargets = false;
    }

    IEnumerator DetectClosestTargetRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(updateInterval);
        
        while (true)
        {
            float closestDist = float.MaxValue;
            closestDamageable = null;
            List<IDamageable> targets = new List<IDamageable>();
            
            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, targetMask);

            foreach (var col in colliders)
            {
                //collider needs to be on same object as IDamageable
                if (col.gameObject.TryGetComponent(out IDamageable damageable))
                    targets.Add(damageable);
            }

            foreach (var target in targets)
            {
                if (IsIDamgeableDestroyed(target))
                {
                    targets.Remove(target);
                    continue;
                }
                
                GameObject targetObject = target.GetGameObject();
                if (targetObject == null) continue;
                
                float dist = Vector3.Distance(target.GetGameObject().transform.position, transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestDamageable = target;
                    closestDamageableDist = dist;
                }
            }
            yield return wait;
        }
    }
    
    bool IsIDamgeableDestroyed(IDamageable damageable)
    {
        if (damageable is not Object unityObject || unityObject == null) return true;
        return false;
    }
}
