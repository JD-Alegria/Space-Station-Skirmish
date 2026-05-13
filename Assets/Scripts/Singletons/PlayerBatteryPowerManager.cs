using UnityEngine;

public class PlayerBatteryPowerManager : MonoBehaviour
{
    public static PlayerBatteryPowerManager Instance;

    int currentBatteryReserves;
    
    
    public int CurrentBatteryReserves => currentBatteryReserves;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    void Start()
    {
        currentBatteryReserves = GameBalanceManager.Instance.StartingBatteryPowerCapacity;
    }

    public void TryIncrementBatteryReserves()
    {
        if (currentBatteryReserves >= GameBalanceManager.Instance.MaxBatteryPowerCapacity) return;

        if (EconomyManager.Instance.TryPurchase(GameBalanceManager.Instance.BatteryPowerUpgradeCost))
        {
            currentBatteryReserves++;
        }
    }

    public void RegisterBatteryControllerEvent(BatteryController batteryController)
    {
        batteryController.OnPowerChanged += HandleBatteryPowerChange;
    }

    public void UnregisterBatteryControllerEvent(BatteryController batteryController)
    {
        currentBatteryReserves += batteryController.CurrentBatteryPowerAllocated;
        batteryController.OnPowerChanged -= HandleBatteryPowerChange;
    }

    void HandleBatteryPowerChange(int batteryPowerChange)
    {
        currentBatteryReserves += batteryPowerChange;
    }
    
}
