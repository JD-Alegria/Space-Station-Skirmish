using System;
using System.Collections;
using UnityEngine;

public class Ship : MonoBehaviour
{
    EnemyShipData data;
    Transform targetPos;
    
    ShipMovement shipMovement;
    ShipHealth shipHealth;
    ShipAttack shipAttack;

    float updateInterval = 0.1f;

    Coroutine commandingFireRoutine;

    void Awake()
    {
        shipMovement = GetComponent<ShipMovement>();
        shipHealth = GetComponent<ShipHealth>();
        shipAttack = GetComponent<ShipAttack>();

        targetPos = GameObject.FindGameObjectWithTag("SpaceStation").transform;
    }

    public void Init(EnemyShipData data)
    {
        this.data = data;
        
        shipHealth.Init(data);
        shipMovement.Init(data, targetPos);
        shipAttack.Init(data);
    }

    void Start()
    {
        if (commandingFireRoutine != null)
        {
            StopCoroutine(commandingFireRoutine);
            commandingFireRoutine = null;
        }

        commandingFireRoutine = StartCoroutine(CommandingFire());
    }

    IEnumerator CommandingFire()
    {
        WaitForSeconds wait = new WaitForSeconds(updateInterval);

        while (true)
        {
            yield return wait;
            if (targetPos == null) continue;
            
            float dist = Vector3.Distance(transform.position, targetPos.position);
            if (dist > data.AttackRange) continue;

            shipAttack.Fire();
        }
    }
}
