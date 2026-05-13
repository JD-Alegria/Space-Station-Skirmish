using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Platform : MonoBehaviour, IInteractable
{
    BatteryController batteryController;
    PlatformData currentPlatformData;
    GameObject currentBuiltPlatform;
    [SerializeField] Transform buildTransform;
    
    public Transform BuildTransform => buildTransform;
    public PlatformData CurrentPlatformData => currentPlatformData;
    public BatteryController BatteryController => batteryController;
    public bool HasPlatform => currentPlatformData != null;
    

    void Awake()
    {
        batteryController = GetComponent<BatteryController>();
    }

    public bool CanInteract(IInteractor interactor)
    {
        return true;
    }

    public void Interact(IInteractor interactor, InteractionType interactionType)
    {
        if (!interactor.Owner.CompareTag("PlayerInteraction")) return;
        
        UIManager.Instance.OpenPlatformUI(this);
    }

    public void Build(PlatformData platformData)
    {
        if (HasPlatform) return;
        if (!EconomyManager.Instance.TryPurchase(platformData.BuildCost)) return;
        
        currentPlatformData = platformData;
        currentBuiltPlatform = Instantiate(
            platformData.PlatformPrefab,
            buildTransform.position,
            Quaternion.identity,
            buildTransform);
        
        //turn on all relevant components
        if (batteryController != null) PlayerBatteryPowerManager.Instance.RegisterBatteryControllerEvent(batteryController);
        batteryController.Init(platformData);

        UIManager.Instance.RefreshPlatformUI(this);
    }

    public void DestroyBuilding()
    {
        if (!HasPlatform) return;
        
        if (batteryController != null) PlayerBatteryPowerManager.Instance.UnregisterBatteryControllerEvent(batteryController);
        
        
        Destroy(currentBuiltPlatform);
        currentBuiltPlatform = null;
        currentPlatformData = null;

        UIManager.Instance.RefreshPlatformUI(this);
    }

    public void IncreasePower()
    {
        if (batteryController == null) return;
        
        batteryController.IncrementBatteryPowerAllocated();
    }

    public void DecreasePower()
    {
        if (batteryController == null) return;
        
        batteryController.DecrementBatteryPowerAllocated();
    }

    public void UpgradeBatteryPowerCapacity()
    {
        if (!gameObject.CompareTag("SpaceStation")) return;
        
        PlayerBatteryPowerManager.Instance.TryIncrementBatteryReserves();
    }

    public void Upgrade()
    {
        throw new NotImplementedException("Not implemented yet");
    }
}
