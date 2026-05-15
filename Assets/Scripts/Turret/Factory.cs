using UnityEngine;
using System.Collections;

public class Factory : MonoBehaviour
{
    [SerializeField] EconomyPlatformData data;
    
    BatteryController batteryController;

    float scrapTickRate;
    int scrapPerTick;
    bool isGeneratingScrap = false;

    Coroutine generateScrap;

    void Start()
    {
        Init(PlatformTierLevel.Lvl1);
        batteryController = GetComponentInParent<BatteryController>();
    }

    public void Init(PlatformTierLevel tierLevel)
    {
        // init per level
        switch (tierLevel)
        {
            case PlatformTierLevel.Lvl1:
                scrapPerTick = data.ScrapPerTickLevel1;
                scrapTickRate = data.ScrapTickRateLevel1;
                break;
            case PlatformTierLevel.Lvl2:
                scrapPerTick = data.ScrapPerTickLevel2;
                scrapTickRate = data.ScrapTickRateLevel2;
                break;
        }
    }

    void Update()
    {
        CheckBatteryPower();
    }

    void CheckBatteryPower()
    {
        if (batteryController.IsPowered && !isGeneratingScrap)
        {
            isGeneratingScrap = true;
            if (generateScrap != null)
            {
                StopCoroutine(generateScrap);
                generateScrap = null;
            }
            generateScrap = StartCoroutine(GenerateScrapRoutine());
        }
        else if (!batteryController.IsPowered && isGeneratingScrap)
        {
            isGeneratingScrap = false;
            if (generateScrap != null)
            {
                StopCoroutine(generateScrap);
                generateScrap = null;
            }
        }
    }

    IEnumerator GenerateScrapRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(scrapTickRate);
        
        while (true)
        {
            yield return wait;
            
            EconomyManager.Instance.AddScrap(scrapPerTick);
        }
    }
}
