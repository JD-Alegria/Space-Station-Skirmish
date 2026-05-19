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
        if (nextUnpoweredBatteryImageIndex >= unpoweredBatteryImages.Count) return;
        
        unpoweredBatteryImages[nextUnpoweredBatteryImageIndex].SetActive(true);
        nextUnpoweredBatteryImageIndex++;
    }

    void HandlePowerAllocationChanged(int powerChange)
    {
        int index = GetNextActiveBatteryIndex();
        
        if (powerChange > 0)
        {
            if (index < 0 || index >= poweredBatteryImages.Count) return;
            poweredBatteryImages[index].SetActive(false);
        }
        
        if (powerChange < 0)
        {
            if (index == -1)
            {
                poweredBatteryImages[0].SetActive(true);
                return;
            }

            int nextIndex = index + 1;
            if (nextIndex >= poweredBatteryImages.Count) return;
            
            poweredBatteryImages[nextIndex].SetActive(true);
        }
    }

    int GetNextActiveBatteryIndex()
    {
        for (int i = 0; i < poweredBatteryImages.Count; i++)
        {
            if (i + 1 > poweredBatteryImages.Count)
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
