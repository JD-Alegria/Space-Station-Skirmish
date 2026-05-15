using UnityEngine;


public class ShieldGenerator : MonoBehaviour
{
    [SerializeField] DefenseSupportPlatformData data;
    
    BatteryController batteryController;

    PlatformTierLevel tierLevel;
    float damageReductionModifier;
    bool registeredToStation = false;
    
    public float DamageReductionModifier => damageReductionModifier;

    void Start()
    {
        Init(PlatformTierLevel.Lvl1);
        batteryController = GetComponentInParent<BatteryController>();
    }

    void Update()
    {
        CheckPowerStatus();
    }

    void Init(PlatformTierLevel tierLevel)
    {
        switch (tierLevel)
        {
            case PlatformTierLevel.Lvl1:
            damageReductionModifier = data.DamageReductionLevel1;
                break;
            case PlatformTierLevel.Lvl2:
            damageReductionModifier = data.DamageReductionLevel2;
                break;
        }
    }

    void CheckPowerStatus()
    {
        if (batteryController.IsPowered && !registeredToStation)
        {
            RegisterShieldGeneratorToStation();
        }
        else if (!batteryController.IsPowered && registeredToStation)
        {
            UnregisterShieldGeneratorToStation();
        }
    }

    void RegisterShieldGeneratorToStation()
    {
        SpaceStationHealth stationHealth = GameObject.FindGameObjectWithTag("SpaceStation").GetComponent<SpaceStationHealth>();
        stationHealth.RegisterShieldGenerator(this);
        registeredToStation = true;
    }

    void UnregisterShieldGeneratorToStation()
    {
        SpaceStationHealth stationHealth = GameObject.FindGameObjectWithTag("SpaceStation").GetComponent<SpaceStationHealth>();
        stationHealth.UnregisterShieldGenerator(this);
        registeredToStation = false;
    }
}
