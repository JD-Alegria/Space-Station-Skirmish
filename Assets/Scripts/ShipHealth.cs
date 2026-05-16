using System;
using UnityEngine;

//thinking about creating a singleton that manages the health and stats of all enemies
//then pass all the data to spawned objects at instantiation
public class ShipHealth : MonoBehaviour, IDamageable
{
    int scrapValue;
    float health;
    
    public event Action<ShipHealth> OnDeath;

    void Awake()
    {
        health = 100f;
    }
    
    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void Init(EnemyShipData data)
    {
        scrapValue = data.ScrapValue;
        health = data.MaxHealth;
    }

    public bool CanTakeDamage { get; }
    public void ApplyDamage(in DamageInfo damageInfo)
    {
        health -= damageInfo.Amount;

        if (health <= 0f)
        {
            OnDeath?.Invoke(this);
        }
    }
}
