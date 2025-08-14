using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float initialSpawnInterval = 5f;
    public int maxEnemies = 10;

    [Header("Difficulty Scaling")]
    public float difficultyIncreaseRate = 0.1f; // How much spawn interval decreases per second
    public float minSpawnInterval = 1f;

    private int currentEnemies = 0;
    private float currentSpawnInterval;
    private List<bool> laneActive;
    private float timeSinceLastDifficultyIncrease;

    void Start()
    {
        currentSpawnInterval = initialSpawnInterval;
        laneActive = new List<bool>();
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            laneActive.Add(true);
        }
        timeSinceLastDifficultyIncrease = Time.time;
        StartCoroutine(SpawnEnemies());
    }

    void Update()
    {
        // Increase difficulty over time if dark crystals are not destroyed
        if (Time.time - timeSinceLastDifficultyIncrease >= 1f) // Every second
        {
            IncreaseDifficulty();
            timeSinceLastDifficultyIncrease = Time.time;
        }
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(currentSpawnInterval);

            if (currentEnemies < maxEnemies)
            {
                List<int> activeLanes = new List<int>();
                for (int i = 0; i < laneActive.Count; i++)
                {
                    if (laneActive[i])
                    {
                        activeLanes.Add(i);
                    }
                }

                if (activeLanes.Count > 0)
                {
                    int spawnPointIndex = activeLanes[Random.Range(0, activeLanes.Count)];
                    Instantiate(enemyPrefab, spawnPoints[spawnPointIndex].position, Quaternion.identity);
                    currentEnemies++;
                }
            }
        }
    }

    public void EnemyDied()
    {
        currentEnemies--;
    }

    public void StopSpawningFromLane(int index)
    {
        if (index >= 0 && index < laneActive.Count)
        {
            laneActive[index] = false;
            Debug.Log("Stopped spawning from lane: " + index);
        }
    }

    void IncreaseDifficulty()
    {
        // Only increase difficulty if there are active lanes
        bool anyLaneActive = false;
        foreach (bool active in laneActive)
        {
            if (active)
            {
                anyLaneActive = true;
                break;
            }
        }

        if (anyLaneActive)
        {
            currentSpawnInterval = Mathf.Max(minSpawnInterval, currentSpawnInterval - difficultyIncreaseRate);
            Debug.Log("Spawn Interval: " + currentSpawnInterval);
        }
    }
}

