using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ShipStatus
{
    Active,
    Destroyed
}

public class Ship : MonoBehaviour
{
    AudioSource audioSource;

    [SerializeField] List<GameObject> deathFireVFX;
    
    EnemyShipData data;
    Transform targetPos;
    ShipStatus shipStatus;
    
    ShipMovement shipMovement;
    ShipHealth shipHealth;
    ShipAttack shipAttack;

    float updateInterval = 0.1f;

    Coroutine commandingFireRoutine;
    Coroutine controlledDeathCoroutine;

    void Awake()
    {
        shipMovement = GetComponent<ShipMovement>();
        shipHealth = GetComponent<ShipHealth>();
        shipAttack = GetComponent<ShipAttack>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Init(EnemyShipData data, Transform lookDirection)
    {
        this.data = data;
        targetPos = lookDirection;
        shipStatus = ShipStatus.Active;
        
        shipHealth.Init(data);
        shipMovement.Init(data, lookDirection);
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

    void OnEnable()
    {
        shipHealth.OnDeath += HandleShipDestroyed;
    }

    void OnDisable()
    {
        shipHealth.OnDeath -= HandleShipDestroyed;
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

    void HandleShipDestroyed(ShipHealth shipHealth)
    {
        if (shipStatus == ShipStatus.Destroyed) return;
        shipStatus = ShipStatus.Destroyed;
        
        switch (data.ShipType)
        {
            case ShipType.Fighter:
                InstantDeath();
                break;
            case ShipType.Corvette:
                controlledDeathCoroutine = StartCoroutine(ControlledDeathRoutine());
                break;
        }
    }

    void PlayExplosionEffects()
    {
        Instantiate(data.DeathVFXPrefab, transform.position, transform.rotation);
        
        int deathSFXIndex = Random.Range(0, data.DeathSFXs.Count);
        audioSource.PlayOneShot(data.DeathSFXs[deathSFXIndex], 0.66f);
    }

    void InstantDeath()
    {
        shipMovement.enabled = false;
        shipAttack.enabled = false;
        EconomyManager.Instance.AddScrap(data.ScrapValue);
        PlayExplosionEffects();
        Destroy(gameObject);
    }

    IEnumerator ControlledDeathRoutine()
    {
        shipAttack.enabled = false;
        
        //play death effects
        foreach (var fire in deathFireVFX)
        {
            fire.SetActive(true);
        }
        int deathSFXIndex = Random.Range(0, data.DeathSFXs.Count);
        audioSource.PlayOneShot(data.DeathSFXs[deathSFXIndex], 0.66f);
        
        yield return shipMovement.SlowToStop();
        Destroy(gameObject);
    }
}
