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
        platform.IncreasePower();
    }

    public void OnDecreasePowerClicked()
    {
        platform.DecreasePower();
    }

    public void OnDestroyClicked()
    {
        platform.DestroyBuilding();
    }

    public void OnUpgradeClicked()
    {
        
    }
}
