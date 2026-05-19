using UnityEngine;
using Michsky.MUIP;

public class StationHealthProgressUI : MonoBehaviour
{
    [SerializeField] ProgressBar progressBar;

    [SerializeField] SpaceStationHealth spaceStationHealth;

    void Start()
    {
        float percent = GameBalanceManager.Instance.StartingBaseHealth <= 0 ? 0f : (GameBalanceManager.Instance.StartingBaseHealth / GameBalanceManager.Instance.StartingBaseHealth) * 100f;
        progressBar.SetValue(percent);
    }
    
    void OnEnable()
    {
        spaceStationHealth.OnDamageTaken += HandleProgressChanged;
        progressBar.isOn = false;
        progressBar.SetValue(0);
    }

    void OnDisable()
    {
        spaceStationHealth.OnDamageTaken -= HandleProgressChanged;
    }

    void HandleProgressChanged(float currentHealth, float startingHealth)
    {
        float percent = currentHealth <= 0 ? 0f : (currentHealth / startingHealth) * 100f;
        progressBar.SetValue(percent);
    }
}
