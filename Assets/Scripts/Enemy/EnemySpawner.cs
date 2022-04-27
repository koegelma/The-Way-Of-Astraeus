using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject stageClearedUI;
    [SerializeField] private Wave[] waves;
    [SerializeField] private Transform[] spawnpoints;
    private int waveIndex = 0;
    private int[] enemiesToSpawn;
    public static int enemiesToDie;
    private float countdown = 3;

    private void Start()
    {
        SpawnWave();
    }

    private void Update()
    {
        if (GameManager.instance.gameEnded) StopAllCoroutines();
        if (enemiesToDie <= 0)
        {
            if (countdown <= 0)
            {
                waveIndex++;
                if (waveIndex == waves.Length)
                {
                    Debug.Log("Level won!");
                    StopAllCoroutines();
                    stageClearedUI.SetActive(true);
                    this.enabled = false;
                    return;
                }
                SpawnWave();
            }
            countdown -= Time.deltaTime;
        }
    }

    private void SpawnWave()
    {
        Wave wave = waves[waveIndex];

        countdown = waves[waveIndex].timeBetweenNextWave;

        enemiesToSpawn = new int[waves[waveIndex].enemies.Length];
        enemiesToDie = 0;

        for (int i = 0; i < waves[waveIndex].enemies.Length; i++)
        {
            enemiesToSpawn[i] = waves[waveIndex].enemies[i].count;
            enemiesToDie += enemiesToSpawn[i];
            StartCoroutine(SpawnEnemies(i));
        }
    }

    private IEnumerator SpawnEnemies(int _enemiesIndex)
    {
        Enemy enemy = waves[waveIndex].enemies[_enemiesIndex];

        Instantiate(enemy.enemyPrefab, spawnpoints[Spawnpoint()].position, transform.rotation);
        enemiesToSpawn[_enemiesIndex]--;

        if (enemiesToSpawn[_enemiesIndex] > 0)
        {
            yield return new WaitForSeconds(enemy.timeBetweenSpawns);
            StartCoroutine(SpawnEnemies(_enemiesIndex));
        }
    }

    private int Spawnpoint()
    {
        return Random.Range(0, spawnpoints.Length);
    }
}
