using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryPowerUI : MonoBehaviour
{
    public static BatteryPowerUI Instance;
    
    [SerializeField] List<GameObject> unpoweredBatteryImages;
    [SerializeField] List<GameObject> poweredBatteryImages;
    
    int nextUnpoweredBatteryImageIndex = 4;
    
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else Instance = this;
    }

    void OnEnable()
    {
        PlayerBatteryPowerManager.OnBatteryReservesUpgraded += HandleBatteryReservesChanged;
        PlayerBatteryPowerManager.OnBatteryAllocationChanged += HandlePowerAllocationChanged;
    }

    void OnDisable()
    {
        PlayerBatteryPowerManager.OnBatteryReservesUpgraded -= HandleBatteryReservesChanged;
        PlayerBatteryPowerManager.OnBatteryAllocationChanged -= HandlePowerAllocationChanged;
    }

    void HandleBatteryReservesChanged()
    {
        if (nextUnpoweredBatteryImageIndex + 1 > unpoweredBatteryImages.Count) return;
        
        unpoweredBatteryImages[nextUnpoweredBatteryImageIndex].SetActive(true);
        nextUnpoweredBatteryImageIndex++;
    }

    void HandlePowerAllocationChanged(int powerChange)
    {
        if (powerChange > 0)
        {
            //power reserves decreased
            poweredBatteryImages[GetNextActiveBatteryIndex()].SetActive(false);
        }
        
        // power reserves increased
        if (powerChange < 0)
        {
            if (GetNextActiveBatteryIndex() + 1 > poweredBatteryImages.Count) return;
            if (GetNextActiveBatteryIndex() == -1)
            {
                poweredBatteryImages[0].SetActive(true);
                return;
            }
            
            poweredBatteryImages[GetNextActiveBatteryIndex() + 1].SetActive(true);
        }
    }

    int GetNextActiveBatteryIndex()
    {
        for (int i = 0; i < poweredBatteryImages.Count; i++)
        {
            if (i + 1 >= poweredBatteryImages.Count)
                return i;
            
            if (poweredBatteryImages[i].activeSelf)
            {
                if (!poweredBatteryImages[i + 1].activeSelf)
                return i;
            }
        }
        return -1;
    }
}
