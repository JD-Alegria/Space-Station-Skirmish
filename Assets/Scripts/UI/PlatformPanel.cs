using UnityEngine;
using UnityEngine.UI;

public class PlatformPanel : MonoBehaviour
{
    protected Platform platform;
    
    
    public virtual void Init(Platform selectedPlatform)
    {
        platform = selectedPlatform;
    }
    
    public void OnIncreasePowerClicked()
    {
        if (platform.IsBuilding) return;
        
        platform.IncreasePower();
    }

    public void OnDecreasePowerClicked()
    {
        if (platform.IsBuilding) return;
        
        platform.DecreasePower();
    }

    public void OnDestroyClicked()
    {
        if (platform.IsBuilding) return;
        
        platform.DestroyBuilding();
    }

    public void OnUpgradeClicked()
    {
        
    }
}
