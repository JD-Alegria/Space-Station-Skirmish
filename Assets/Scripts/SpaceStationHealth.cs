using Unity.Cinemachine;
using UnityEngine;
using System;
using System.Collections.Generic;

public class SpaceStationHealth : MonoBehaviour, IDamageable
{
    List<ShieldGenerator> poweredShieldGenerators = new ();
    bool canTakeDamage = true;
    float baseHealth;
    float startingHealth;
    float damageReductionModifer;
    
    public float BaseHealth => baseHealth;

    public float DamageReductionModifer => damageReductionModifer;

    public event Action<float, float> OnDamageTaken;
    public event Action OnDestroyed;

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public bool CanTakeDamage { get; }

    void Start()
    {
        baseHealth = GameBalanceManager.Instance.StartingBaseHealth;
        startingHealth = baseHealth;
    }

    public void RegisterShieldGenerator(ShieldGenerator shieldGenerator)
    {
        poweredShieldGenerators.Add(shieldGenerator);
    }

    public void UnregisterShieldGenerator(ShieldGenerator shieldGenerator)
    {
        poweredShieldGenerators.Remove(shieldGenerator);
    }
    
    public void ApplyDamage(in DamageInfo damageInfo)
    {
        float totalReduction = 0f;
        foreach (var shieldGenerator in poweredShieldGenerators)
        {
            totalReduction += shieldGenerator.DamageReductionModifier;
        }
        totalReduction = Mathf.Clamp01(totalReduction);
        float finalDamage = damageInfo.Amount * (1f - totalReduction);
        
        baseHealth -= finalDamage;
        
        OnDamageTaken?.Invoke(baseHealth, startingHealth);

        if (baseHealth <= 0) Die();
    }

    void Die()
    {
        OnDestroyed?.Invoke();
        Destroy(gameObject);
    }
}
