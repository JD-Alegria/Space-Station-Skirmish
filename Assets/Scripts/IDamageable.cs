using UnityEngine;

public interface IDamageable
{
    GameObject GetGameObject();
    
    bool CanTakeDamage { get; }
    // `in` passes the struct by readonly reference.
    // That means no copy is made, and ApplyDamage cannot modify the DamageInfo
    void ApplyDamage(in DamageInfo damageInfo);
}

// Struct = a lightweight value type used to group related damage data together.
// For simple cases, only Amount may be used. Source and HitPoint are optional context.
public readonly struct DamageInfo
{
    public readonly float Amount;
    public readonly GameObject Source;
    public readonly Vector3 HitPoint;
    
    // `default` means "use the type's default value".
    // For Vector3, that is (0, 0, 0).
    public DamageInfo(float amount, GameObject source = null, Vector3 hitPoint = default)
    {
        Amount = amount;
        Source = source;
        HitPoint = hitPoint;
    }
}