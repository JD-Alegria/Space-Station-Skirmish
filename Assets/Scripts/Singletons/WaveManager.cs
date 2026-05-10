using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public enum WaveState
{
    Spawning,
    Active,
    Paused
}

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;
    public static WaveState CurrentWaveState;

    float updateInterval = 0.1f;
    int activeShipCount = 0;
    List<ShipHealth> activeShips = new List<ShipHealth>();
    
    public int ActiveShipCount => activeShipCount;

    Coroutine waveTrackerRoutine;
    public event Action<WaveState> OnWaveStateChange;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    void Start()
    {
        CurrentWaveState = WaveState.Paused;
        
        if (waveTrackerRoutine != null)
        {
            StopCoroutine(waveTrackerRoutine);
            waveTrackerRoutine = null;
        }

        waveTrackerRoutine = StartCoroutine(WaveTrackerRoutine());
    }

    void OnEnable()
    {
        UIManager.Instance.OnNextWavePressed += HandleWaveStateLogic;
    }

    void OnDisable()
    {
        UIManager.Instance.OnNextWavePressed -= HandleWaveStateLogic;
    }

    IEnumerator WaveTrackerRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(updateInterval);

        while (true)
        {
            yield return wait;
            
            activeShipCount = activeShips.Count;

            switch (CurrentWaveState)
            {
                case WaveState.Spawning:
                    if (activeShipCount >= SpawnManager.Instance.OneShotShipsToSpawn)
                        ChangeWaveState(WaveState.Active);
                    break;
                case WaveState.Active:
                    if (activeShipCount <= 0)
                        ChangeWaveState(WaveState.Paused);
                    break;
                case WaveState.Paused:
                    break;
            }
        }
    }

    void ChangeWaveState(WaveState newWaveState)
    {
        CurrentWaveState = newWaveState;
        OnWaveStateChange?.Invoke(CurrentWaveState);
    }

    void HandleWaveStateLogic()
    {
        switch (CurrentWaveState)
        {
            case WaveState.Spawning:
                break;
            case WaveState.Active:
                break;
            case WaveState.Paused:
                ChangeWaveState(WaveState.Spawning);
                break;
        }
    }

    public void RegisterShip(ShipHealth ship)
    {
        if (ship == null) return;
        ship.OnDeath += HandleShipDestroyed;
        activeShips.Add(ship);
    }

    void HandleShipDestroyed(ShipHealth ship)
    {
        activeShips.Remove(ship);
        ship.OnDeath -= HandleShipDestroyed;

        if (activeShips.Count <= 0)
        {
            ChangeWaveState(WaveState.Paused);
        }
    }
}
