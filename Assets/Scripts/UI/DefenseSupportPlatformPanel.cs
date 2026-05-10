using UnityEngine;
using UnityEngine.UI;

public class DefenseSupportPlatformPanel : PlatformPanel
{
    [SerializeField] protected Image icon;
    
    public override void Init(Platform selectedPlatform)
    {
        base.Init(selectedPlatform);
        icon.sprite = selectedPlatform.CurrentPlatformData.Icon;
    }
}
