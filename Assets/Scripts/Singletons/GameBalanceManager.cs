using UnityEngine;

public class GameBalanceManager : MonoBehaviour
{
    public static GameBalanceManager Instance;
    
    [SerializeField] float startingBaseHealth = 100f;
    [SerializeField] int startingMoney = 100;
    [SerializeField] int startingBatteryPowerCapacity = 3;
    [SerializeField] int maxBatteryPowerCapacity = 10;
    [SerializeField] int batteryPowerUpgradeCost = 10;
    [SerializeField] int numberOfWavesToWin = 5;

    public int StartingMoney => startingMoney;
    public float StartingBaseHealth => startingBaseHealth;
    public int StartingBatteryPowerCapacity => startingBatteryPowerCapacity;
    public int BatteryPowerUpgradeCost => batteryPowerUpgradeCost;
    public int MaxBatteryPowerCapacity => maxBatteryPowerCapacity;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else Instance = this;
    }
}
