using Unity.Cinemachine;
using UnityEngine;
using System;

public class SpaceStationHealth : MonoBehaviour, IDamageable
{
    bool canTakeDamage = true;
    float baseHealth;

    public event Action OnDestroyed;

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public bool CanTakeDamage { get; }

    void Start()
    {
        baseHealth = GameBalanceManager.Instance.StartingBaseHealth;
    }
    
    public void ApplyDamage(in DamageInfo damageInfo)
    {
        baseHealth -= damageInfo.Amount;

        if (baseHealth <= 0) Die();
    }

    void Die()
    {
        OnDestroyed?.Invoke();
        Destroy(gameObject);
    }
}
