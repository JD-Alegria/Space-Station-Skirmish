using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;
    
    [Header("Data")]
    [SerializeField] EnemyShipData fighterData;
    [SerializeField] EnemyShipData corvetteData;

    [Space]
    [SerializeField] List<Transform> spawnPos;
    [SerializeField] List<GameObject> spawnPrefabs;
    [SerializeField] Transform spaceStationTransform;
    [SerializeField] float spawnDelay = 1f;

    bool isSpawning = false;
    
    public bool IsSpawning => isSpawning;

    [Header("Debug Force Spawn")]
    [SerializeField] bool forceSpawnFightersOnly = false;
    [SerializeField] bool forceSpawnCorvettesOnly = false;

    public static event Action<int, int> OnWaveSpawnProgressChanged;
    Coroutine spawnRoutine;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    void Start()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }
    }

    void OnEnable()
    {
        WaveManager.Instance.OnWaveStateChange += SpawnWave;
    }

    void OnDisable()
    {
        WaveManager.Instance.OnWaveStateChange -= SpawnWave;
    }

    void SpawnWave(WaveState newWaveState, int waveCount, int threatBudget)
    {
        if (newWaveState != WaveState.Spawning) return;
        
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }
        
        //Debug code only
        if (forceSpawnFightersOnly)
        {
            spawnRoutine = StartCoroutine(SpawnRandomShip(threatBudget, 0));
            return;
        }
        
        if (forceSpawnCorvettesOnly)
        {
            spawnRoutine = StartCoroutine(SpawnRandomShip(threatBudget, 1));
            return;
        }

        //only spawnfighters
        if (waveCount <= GameBalanceManager.Instance.WaveLevelToStartSpawningCorvettes)
        {
            spawnRoutine = StartCoroutine(SpawnRandomShip(threatBudget, 0));
            return;
        }
        
        spawnRoutine = StartCoroutine(SpawnRandomShip(threatBudget));
    }

    //has debug code, needs to be refactored to include how many ships to spawn
    IEnumerator SpawnRandomShip(int threatBudget, int spawnIndex = -1)
    {
        if (spawnPrefabs.Count == 0) yield break;
        if (WaveManager.CurrentWaveState != WaveState.Spawning) yield break;

        int threatBudgetCache = threatBudget;
        bool isSpawnRandom;
        if (spawnIndex == -1) isSpawnRandom = true;
        else isSpawnRandom = false;
        isSpawning = true;
        
        while (threatBudget > 0)
        {
            WaitForSeconds wait = new WaitForSeconds(spawnDelay);
            
            if (isSpawnRandom)
            {
                spawnIndex = Random.Range(0, spawnPrefabs.Count);
            }

            if (spawnIndex == 0)
            {
                    Vector3 spawnPos = GetRandomSpawnPos();
                    Quaternion lookDirection = GetLookDirection(spawnPos);
                    SpawnFighter(spawnIndex, spawnPos, lookDirection);
                    threatBudget -= fighterData.ThreatCost;
                    OnWaveSpawnProgressChanged?.Invoke(threatBudgetCache - threatBudget, threatBudgetCache);
                    yield return wait;
            }
            else if (spawnIndex == 1)
            {
                    Vector3 spawnPos = GetRandomSpawnPos();
                    Quaternion lookDirection = GetLookDirection(spawnPos);
                    SpawnCorvette(spawnIndex, spawnPos, lookDirection);
                    threatBudget -= corvetteData.ThreatCost;
                    OnWaveSpawnProgressChanged?.Invoke(threatBudgetCache - threatBudget, threatBudgetCache);
                    yield return wait;
            }
        }
        isSpawning = false;
        spawnRoutine = null;
    }

    void SpawnFighter(int index, Vector3 spawnPos, Quaternion lookDirection)
    {
        GameObject newFighter = Instantiate(spawnPrefabs[index], spawnPos, lookDirection);
        Ship newFighterShip = newFighter.GetComponent<Ship>();
        newFighterShip.Init(fighterData, spaceStationTransform);
        
        WaveManager.Instance.RegisterShip(newFighterShip.GetComponent<ShipHealth>());
    }

    void SpawnCorvette(int index, Vector3 spawnPos, Quaternion lookDirection)
    {
        GameObject newCorvette = Instantiate(spawnPrefabs[index], spawnPos, lookDirection);
        Ship newCorvetteShip = newCorvette.GetComponent<Ship>();
        newCorvetteShip.Init(corvetteData, spaceStationTransform);
        
        WaveManager.Instance.RegisterShip(newCorvette.GetComponent<ShipHealth>());
    }

    Vector3 GetRandomSpawnPos()
    {
        if (spawnPos.Count == 0) return new Vector3();

        int index = Random.Range(0, spawnPos.Count);
        Vector3 randomOffset = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
        
        return spawnPos[index].position + randomOffset;
    }

    Quaternion GetLookDirection(Vector3 spawnPos)
    {
        if (spaceStationTransform == null) return Quaternion.identity;
        
        Vector3 direction = spaceStationTransform.position - spawnPos;
        return Quaternion.LookRotation(direction);
    }
}
