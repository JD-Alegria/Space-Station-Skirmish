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

    [Space]
    [SerializeField] List<Transform> spawnPos;
    [SerializeField] List<GameObject> spawnPrefabs;
    [SerializeField] Transform spaceStationTransform;
    [Tooltip("Spawns Ships, then ends SpawnManager. If zero, then ignored. Primarily a Debug Tool.")]
    [SerializeField] int oneShotShipsToSpawn;
    [SerializeField] float spawnDelay = 1f;
    
    //debug tool for now
    public int OneShotShipsToSpawn => oneShotShipsToSpawn;

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

        spawnRoutine = StartCoroutine(SpawnRandomSingleObjectType());
    }

    void OnEnable()
    {
        WaveManager.Instance.OnWaveStateChange += SpawnWave;
    }

    void OnDisable()
    {
        WaveManager.Instance.OnWaveStateChange -= SpawnWave;
    }

    void SpawnWave(WaveState newWaveState)
    {
        if (newWaveState != WaveState.Spawning) return;
        
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }
        
        spawnRoutine = StartCoroutine(SpawnRandomSingleObjectType());
    }

    //has debug code, needs to be refactored to include how many ships to spawn
    IEnumerator SpawnRandomSingleObjectType()
    {
        if (spawnPrefabs.Count == 0) yield break;
        if (WaveManager.CurrentWaveState != WaveState.Spawning) yield break;
        
        WaitForSeconds wait = new WaitForSeconds(spawnDelay);
        int index = Random.Range(0, spawnPrefabs.Count);

        for (int i = 0; i < oneShotShipsToSpawn; i++)
        {
            Vector3 spawnPos = GetRandomSpawnPos();
            Quaternion lookDirection = GetLookDirection(spawnPos);
            SpawnFighter(index, spawnPos, lookDirection);
            yield return wait;
        }
        
        spawnRoutine = null;
    }

    void SpawnFighter(int index, Vector3 spawnPos, Quaternion lookDirection)
    {
        GameObject newFighter = Instantiate(spawnPrefabs[index], spawnPos, lookDirection);
        Ship newFighterShip = newFighter.GetComponent<Ship>();
        newFighterShip.Init(fighterData);
        
        WaveManager.Instance.RegisterShip(newFighterShip.GetComponent<ShipHealth>());
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
