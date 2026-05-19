using UnityEngine;
using System.Collections;
using System;

public class BatteryController : MonoBehaviour
{
    [SerializeField] PlatformData platformData;

    PlatformTierLevel platformTierLevel = PlatformTierLevel.Lvl1;
    float updateInterval = .05f;
    [SerializeField] int currentBatteryPowerAllocated = 0;
    [SerializeField] bool isPowered = false;

    public bool IsPowered => isPowered;
    public int CurrentBatteryPowerAllocated => currentBatteryPowerAllocated;

    public event Action<int> OnPowerChanged;

    Coroutine checkBatteryPowerRoutine;

    void Start()
    {
        if (checkBatteryPowerRoutine != null)
        {
            StopCoroutine(checkBatteryPowerRoutine);
            checkBatteryPowerRoutine = null;
        }
        
        checkBatteryPowerRoutine = StartCoroutine(CheckBatteryPower());
    }

    void Update()
    {
        if (platformData == null) return;
        
        
    }

    public void Init(PlatformData data)
    {
        platformTierLevel = PlatformTierLevel.Lvl1;
        currentBatteryPowerAllocated = 0;
        isPowered = false;
        platformData = data;
        
        if (checkBatteryPowerRoutine != null)
        {
            StopCoroutine(checkBatteryPowerRoutine);
            checkBatteryPowerRoutine = null;
        }
        
        checkBatteryPowerRoutine = StartCoroutine(CheckBatteryPower());
    }

    public void Reset()
    {
        //reset all values when destroyed
    }

    public void IncrementBatteryPowerAllocated()
    {
        switch (platformTierLevel)
        {
            case PlatformTierLevel.Lvl1:
                if (currentBatteryPowerAllocated >= platformData.BatteryPowerCostLevel1)
                {
                    return;
                }

                if (PlayerBatteryPowerManager.Instance.CurrentBatteryReserves <= 0)
                {
                    return;
                }
                currentBatteryPowerAllocated++;
                OnPowerChanged?.Invoke(-1);
                break;
            case PlatformTierLevel.Lvl2:
                if (currentBatteryPowerAllocated >= platformData.BatteryPowerCostLevel2)
                {
                    return;
                }
                if (PlayerBatteryPowerManager.Instance.CurrentBatteryReserves <= 0)
                {
                    return;
                }
                currentBatteryPowerAllocated++;
                OnPowerChanged?.Invoke(-1);
                break;
        }
    }

    public void DecrementBatteryPowerAllocated()
    {
        if (currentBatteryPowerAllocated <= 0)
        {
            return;
        }
        currentBatteryPowerAllocated--;
        OnPowerChanged?.Invoke(1);
    }

    IEnumerator CheckBatteryPower()
    {
            WaitForSeconds wait = new WaitForSeconds(updateInterval);
        
        while (true)
        {
            if (platformData == null) yield break;
            

            switch (platformTierLevel)
            {
                case (PlatformTierLevel.Lvl1):
                    if (currentBatteryPowerAllocated < platformData.BatteryPowerCostLevel1)
                    {
                        isPowered = false;
                        break;
                    }
                    isPowered = true;
                    break;
                case (PlatformTierLevel.Lvl2):
                    if (currentBatteryPowerAllocated < platformData.BatteryPowerCostLevel2) isPowered = false;
                    isPowered = true;
                    break;
            }
            yield return wait;
        }
    }
}
