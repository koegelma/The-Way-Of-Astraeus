using UnityEngine;

[System.Serializable]
public class Enemy
{
    public GameObject enemyPrefab;
    public int count;
    public int difficulty;
    public float timeBetweenFirstSpawn;
    public float timeBetweenSpawns;
}
