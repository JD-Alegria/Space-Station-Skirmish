using System.Collections;
using UnityEngine;
using Michsky.MUIP;
using Unity.Mathematics;

public class WaveSpawnProgressUI : MonoBehaviour
{
    [SerializeField] ProgressBar progressBar;
    [SerializeField] float fillSpeed = 40f;

    Coroutine fillRoutine;

    void OnEnable()
    {
        SpawnManager.OnWaveSpawnProgressChanged += HandleProgressChanged;
        progressBar.isOn = false;
        progressBar.SetValue(0);
    }

    void OnDisable()
    {
        SpawnManager.OnWaveSpawnProgressChanged -= HandleProgressChanged;
    }

    void HandleProgressChanged(int threatBudgetUsed, int threatBudget)
    {
        float percent = threatBudget <= 0 ? 0f : ((float)threatBudgetUsed / threatBudget) * 100f;
        if (fillRoutine != null)
        {
            StopCoroutine(fillRoutine);
            fillRoutine = null;
        }

        fillRoutine = StartCoroutine((AnimateTo(percent)));
    }

    IEnumerator AnimateTo(float percent)
    {
        while (!Mathf.Approximately(progressBar.currentPercent, percent))
        {
            float nextValue = Mathf.MoveTowards(
                progressBar.currentPercent,
                percent,
                fillSpeed * Time.unscaledDeltaTime);
            
            progressBar.SetValue(nextValue);
            yield return null;
        }

        progressBar.SetValue(percent);
        fillRoutine = null;
    }
}
