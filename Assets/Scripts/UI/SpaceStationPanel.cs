using UnityEngine;

public class SpaceStationPanel : PlatformPanel
{
    public override void Init(Platform selectedPlatform)
    {
        platform = selectedPlatform;
    }

    public void OnBatteryCapacityClick()
    {
        platform.UpgradeBatteryPowerCapacity();
    }
}
