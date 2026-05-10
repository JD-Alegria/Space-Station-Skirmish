using UnityEngine;

[CreateAssetMenu(fileName = "EconomyPlatformData", menuName = "Scriptable Objects/EconomyPlatformData")]
public class EconomyPlatformData : PlatformData
{
    [Header("Economy Platform Stats")]
    [SerializeField] float scrapIntervalLevel1;
    [SerializeField] float scrapIntervalLevel2;
    [SerializeField] float scrapPerTickRateLevel1;
    [SerializeField] float scrapPerTickRateLevel2;
    
    public float ScrapIntervalLevel1 => scrapIntervalLevel1;
    public float ScrapIntervalLevel2 => scrapIntervalLevel2;
    public float ScrapPerTickRateLevel1 => scrapPerTickRateLevel1;
    public float ScrapPerTickRateLevel2 => scrapPerTickRateLevel2;
}
