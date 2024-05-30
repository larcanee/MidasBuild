using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab1; // Prefab of the first type of enemy
    public GameObject enemyPrefab2; // Prefab of the second type of enemy
    public float spawnIntervalMin = 2f; // Minimum time between spawns
    public float spawnIntervalMax = 5f; // Maximum time between spawns
    public int initialEnemies = 5; // Number of enemies to spawn at the start
    public bool stop = false;
    public List<Transform> spawnpoints;

    private float nextSpawnTime; // Time to spawn the next enemy

    public EnemyCounter enemyCounter_;

    public bool start = false;

    void Start()
    {
        // // Spawn the initial wave of enemies
        // for (int i = 0; i < initialEnemies; i++)
        // {
        //     SpawnEnemy();
        // }
        // // Schedule the first spawn
        // nextSpawnTime = Time.time + Random.Range(spawnIntervalMin, spawnIntervalMax);
    }

    public void StartSpawn()
    {
        // Spawn the initial wave of enemies
        for (int i = 0; i < initialEnemies; i++)
        {
            SpawnEnemy();
        }
        // Schedule the first spawn
        nextSpawnTime = Time.time + Random.Range(spawnIntervalMin, spawnIntervalMax);
        start = true;
    }

    void Update()
    {
        // Check if it's time to spawn a new enemy
        if (Time.time >= nextSpawnTime && !stop && start)
        {
            SpawnEnemy();

            // Schedule the next spawn time
            nextSpawnTime = Time.time + Random.Range(spawnIntervalMin, spawnIntervalMax);
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
        enemy.GetComponent<Health>().enemyCounter = enemyCounter_;
    }
}
