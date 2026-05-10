using UnityEngine;
using UnityEngine.UI;

public class EconomyPlatformPanel : PlatformPanel
{
    [SerializeField] protected Image icon;
    
    public override void Init(Platform selectedPlatform)
    {
        base.Init(selectedPlatform);
        icon.sprite = selectedPlatform.CurrentPlatformData.Icon;
    }
}
