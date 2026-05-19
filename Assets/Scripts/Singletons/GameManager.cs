using System;
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] SpaceStationHealth spaceStationHealth;
    [SerializeField] GameObject gameOverPanel;

    float updateInterval = 0.1f;
    bool isGameOver;

    public Action OnGameOver;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
        
        isGameOver = false;
    }

    void OnEnable()
    {
        spaceStationHealth.OnDestroyed += HandleSpaceStationHealthDeath;
    }

    void OnDisable()
    {
        spaceStationHealth.OnDestroyed -= HandleSpaceStationHealthDeath;
    }

    void HandleSpaceStationHealthDeath()
    {
        if (isGameOver) return;
        
        isGameOver = true;
        gameOverPanel.SetActive(true);
        OnGameOver?.Invoke();
    }
}
