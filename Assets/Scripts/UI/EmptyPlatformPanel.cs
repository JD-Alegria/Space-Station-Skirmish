using System;
using UnityEngine;

public class EmptyPlatformPanel : PlatformPanel
{
    [SerializeField] PlatformData gunTurretPlatformData;
    [SerializeField] PlatformData ionTurretPlatformData;
    [SerializeField] PlatformData defensePlatformData;
    [SerializeField] PlatformData economyPlatformData;

    public void OnBuildGunTurretPlatformClicked()
    {
        if (platform == null) return;
        
        platform.Build(gunTurretPlatformData);
    }

    public void OnBuildIonTurretPlatformClicked()
    {
        if  (platform == null) return;
        
        platform.Build(ionTurretPlatformData);
    }

    public void OnBuildDefensePlatformClicked()
    {
        throw new NotImplementedException();
        platform.Build(defensePlatformData);
    }

    public void OnBuildEconomyPlatformClicked()
    {
        throw new NotImplementedException();
        platform.Build(economyPlatformData);
    }
}
