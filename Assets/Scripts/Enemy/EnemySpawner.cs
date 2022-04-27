using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnpoints;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float timeBetweenSpawns;
    private float spawnTimer = 1;

    private void Update()
    {
        if (GameManager.instance.gameEnded) return;
        if (spawnTimer <= 0)
        {
            SpawnEnemy();
            return;
        }
        spawnTimer -= Time.deltaTime;
    }

    private void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnpoints[Spawnpoint()].position, transform.rotation);
        spawnTimer = timeBetweenSpawns;
    }

    private int Spawnpoint()
    {
        return Random.Range(0, spawnpoints.Length);
    }
}
