using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject stageClearedUI;
    [SerializeField] private Text waveCountdown;
    [SerializeField] private Wave[] waves;
    private Transform[] spawnpoints;
    private int waveIndex = 0;
    private int[] enemiesToSpawn;
    private float countdown;
    private int Spawnpoint { get { return Random.Range(0, spawnpoints.Length); } }
    public static int enemiesToDie;

    private void Start()
    {
        spawnpoints = new Transform[transform.childCount];
        for (int i = 0; i < spawnpoints.Length; i++)
        {
            spawnpoints[i] = transform.GetChild(i);
        }
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
                waveCountdown.text = null;
                if (waveIndex == waves.Length)
                {
                    StopAllCoroutines();
                    stageClearedUI.SetActive(true);
                    this.enabled = false;
                    return;
                }
                SpawnWave();
                return;
            }
            waveCountdown.text = Mathf.Round(countdown) + "...";
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

        if (enemiesToSpawn[_enemiesIndex] == enemy.count) yield return new WaitForSeconds(enemy.timeBetweenFirstSpawn);

        Instantiate(enemy.enemyPrefab, spawnpoints[Spawnpoint].position, transform.rotation);
        enemiesToSpawn[_enemiesIndex]--;

        if (enemiesToSpawn[_enemiesIndex] > 0)
        {
            yield return new WaitForSeconds(enemy.timeBetweenSpawns);
            StartCoroutine(SpawnEnemies(_enemiesIndex));
        }
    }
}
