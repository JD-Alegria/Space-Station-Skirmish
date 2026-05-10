using System;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    [SerializeField] TMP_Text batteryReserveCounter;
    [SerializeField] TMP_Text scrapCounter;
    [SerializeField] TMP_Text enemyShipsLeftText;
    [SerializeField] GameObject emptyPlatformPanelUIPrefab;
    [SerializeField] GameObject spaceStationPanelUIPrefab;
    [SerializeField] Transform canvasTransform;
    [SerializeField] PlayerInteract playerInteract;

    GameObject currentPlatformPanelUI;

    public event Action OnNextWavePressed;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    void Start()
    {
        playerInteract.OnClickAway += CloseCurrentPlatformUI;
    }

    void OnDestroy()
    {
        playerInteract.OnClickAway -= CloseCurrentPlatformUI;
    }

    void Update()
    {
        UpdateTextOnScreen();
    }

    void UpdateTextOnScreen()
    {
        UpdateBatteryReserveCounter();
        UpdateScrapCounter();
        UpdateShipsLeftText();
    }

    void UpdateBatteryReserveCounter()
    {
        batteryReserveCounter.text = "Battery Reserve: " + PlayerBatteryPowerManager.Instance.CurrentBatteryReserves;
    }

    void UpdateScrapCounter()
    {
        scrapCounter.text = "Scrap: " + EconomyManager.Instance.CurrentScrap;
    }

    void UpdateShipsLeftText()
    {
        enemyShipsLeftText.text = "Enemy Ships Left: " + WaveManager.Instance.ActiveShipCount;
    }

    public void OpenPlatformUI(Platform platform)
    {
        CloseCurrentPlatformUI();
        
        if (!platform.HasPlatform && !platform.gameObject.CompareTag("SpaceStation"))
        {
            OpenBuildPanel(platform);
            return;
        }

        if (platform.HasPlatform)
        {
            OpenCurrentPlatformPanel(platform);
        }

        if (platform.gameObject.CompareTag("SpaceStation"))
        {
            OpenSpaceStationUI(platform);
        }
    }

    public void OpenSpaceStationUI(Platform platform)
    {
        CloseCurrentPlatformUI();
        
        currentPlatformPanelUI = Instantiate(spaceStationPanelUIPrefab, canvasTransform);
        PlatformPanel panel = currentPlatformPanelUI.GetComponent<PlatformPanel>();
        panel.Init(platform);
    }

    public void CloseCurrentPlatformUI()
    {
        if (currentPlatformPanelUI == null) return;
        
        Destroy(currentPlatformPanelUI);
        currentPlatformPanelUI = null;
    }

    public void RefreshPlatformUI(Platform platform)
    {
        
    }

    public void OpenBuildPanel(Platform platform)
    {
        currentPlatformPanelUI = Instantiate(emptyPlatformPanelUIPrefab, canvasTransform);
        PlatformPanel panel = currentPlatformPanelUI.GetComponent<PlatformPanel>();
        panel.Init(platform);
    }

    public void OpenCurrentPlatformPanel(Platform platform)
    {
        currentPlatformPanelUI = Instantiate(platform.CurrentPlatformData.PlatformPanel, canvasTransform);
        PlatformPanel panel = currentPlatformPanelUI.GetComponent<PlatformPanel>();
        panel.Init(platform);
    }

    public void HandleSpawnNextWaveButton()
    {
        OnNextWavePressed?.Invoke();
    }
}
