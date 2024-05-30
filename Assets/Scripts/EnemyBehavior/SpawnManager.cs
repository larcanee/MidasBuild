using System.Collections.Generic;
using UnityEngine;


public class SpawnManager : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject enemyPrefab1 = null;
    public GameObject enemyPrefab2 = null;
    public EnemyCounter enemyCounter = null;

    [Header("Spawn Rate Settings")]
    [Tooltip("How many enemies to spawn at the start of the game.")]
    public int initialEnemies = 5;
    [Tooltip("How long (in seconds) before the late game is considered to begin.")]
    public float lateGameSeconds = 480f;
    [Tooltip(
        "If true, the spawn rate will stop changing once the late game is reached.\n" +
        "If false, the spawn rate will continue decreasing forever, eventually spawning enemies every frame."
    )]
    public bool clampSpawnRates = true;

    [Header("Spawn Rate in the Early Game")]
    public float earlyMinInterval = 5f;
    public float earlyMaxInterval = 10f;

    [Header("Spawn Rate in the Late Game")]
    public float lateMinInterval = 0.25f;
    public float lateMaxInterval = 0.5f;

    [Header("Spawn Points")]
    public List<Transform> spawnpoints = new();


    private float startTime;
    private float nextSpawnTime; // Time to spawn the next enemy
    public bool start = false;


    void Start()
    {
        
    }

    public void StartSpawn()
    {
        startTime = Time.time;
        // Schedule the first spawn
        ScheduleNextSpawnTime();
        // Spawn the initial wave of enemies
        for (int i = 0; i < initialEnemies; i++)
        {
            SpawnEnemy();
        }
        start = true;

    }

    void Update()
    {
        // Check if it's time to spawn a new enemy
        if (Time.time >= nextSpawnTime  && start)
        {
            SpawnEnemy();

            // Schedule the next spawn time
            ScheduleNextSpawnTime();
        }
    }

    void SpawnEnemy()
    {
        // Randomly choose which type of enemy to spawn
        GameObject enemyToSpawn = Random.Range(0, 2) == 0 ? enemyPrefab1 : enemyPrefab2;

        // Calculate a random position on the screen
        //Vector3 spawnPosition = new Vector3(Random.Range(-8f, 8f), Random.Range(-4.5f, 4.5f), 0f);

        //Choose a random spawn point from spawnpoint list (future change to nearest 4)
        //random number for indices
        int index = Random.Range(0, spawnpoints.Count);
        Vector3 spawnPosition = spawnpoints[index].position;

        // Instantiate the chosen type of enemy at the calculated position
        GameObject enemy = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
        enemy.GetComponent<Health>().enemyCounter = enemyCounter;
    }

    void ScheduleNextSpawnTime()
    {
        float timeElapsed = Time.time - startTime;
        float gameProgress = timeElapsed / lateGameSeconds;
        float minInterval;
        float maxInterval;
        if (clampSpawnRates)
        {
            minInterval = Mathf.Lerp(earlyMinInterval, lateMinInterval, gameProgress);
            maxInterval = Mathf.Lerp(earlyMaxInterval, lateMaxInterval, gameProgress);
        }
        else
        {
            minInterval = Mathf.LerpUnclamped(earlyMinInterval, lateMinInterval, gameProgress);
            maxInterval = Mathf.LerpUnclamped(earlyMaxInterval, lateMaxInterval, gameProgress);
        }
        nextSpawnTime = Time.time + Random.Range(minInterval, maxInterval);
    }
}
