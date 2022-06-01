using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private GameObject stageClearedUI;
    [SerializeField] private Text waveCountdown;
    [SerializeField] private Text waveCounter;
    [SerializeField] private Wave[] waves;
    private Transform[] spawnpoints;
    private int waveIndex = 0;
    private int[] enemiesToSpawn;
    private float countdown;
    private int Spawnpoint { get { return Random.Range(0, spawnpoints.Length); } }
    public static int enemiesToDie;

    private void Start()
    {
        gameManager = GameManager.instance;

        spawnpoints = new Transform[transform.childCount];
        for (int i = 0; i < spawnpoints.Length; i++)
        {
            spawnpoints[i] = transform.GetChild(i);
        }
        GenerateEnemies();
        SpawnWave();
        waveCounter.text = (waveIndex + 1) + " | " + waves.Length;
    }

    private void Update()
    {
        if (gameManager.gameEnded) StopAllCoroutines();
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
                    gameManager.gameEnded = true;
                    Cursor.visible = true;
                    this.enabled = false;
                    return;
                }
                waveCounter.text = (waveIndex + 1) + " | " + waves.Length;
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

    private void GenerateEnemies()
    {
        foreach (Wave wave in waves)
        {
            int currentDiff = 0;
            foreach (Enemy enemy in wave.enemies)
            {
                currentDiff += enemy.difficulty * enemy.count;
            }

            while (wave.difficulty > currentDiff)
            {
                int newEnemy = GetRandomEnemy(wave, wave.difficulty - currentDiff);
                currentDiff += wave.enemies[newEnemy].difficulty;
                wave.enemies[newEnemy].count++;
            }
        }
    }

    private int GetRandomEnemy(Wave _wave, int _outstandingDiff)
    {
        int validEnemies = 0;
        foreach (Enemy enemy in _wave.enemies)
        {
            if (enemy.difficulty <= _outstandingDiff) validEnemies++;
        }
        return Random.Range(0, validEnemies);
    }
}
