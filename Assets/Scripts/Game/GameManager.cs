using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private SaveLoadSystem saveLoadSystem;
    private PlayerStats playerStats;
    //private float totalDamage;
    //public float TotalDamage { get { return totalDamage; } }
    public static GameManager instance;
    public bool gameEnded;
    public GameObject playerShip;

    private void Awake()
    {
        instance = this;
        saveLoadSystem.Load();
    }
    private void Start()
    {
        Time.timeScale = 1;
        gameEnded = false;
        playerStats = PlayerStats.instance;
        //totalDamage = 0;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (playerHealth.health <= 0) StartCoroutine(EndGame());
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(1);
        gameEnded = true;
        gameOverUI.SetActive(true);
        Cursor.visible = true;
    }

    /* public void AddTotalDamage(float _damage)
    {
        totalDamage += _damage;
    } */

    public IEnumerator SaveAndReloadScene()
    {
        saveLoadSystem.Save();
        yield return new WaitUntil(() => saveLoadSystem.hasSaved);
        SceneChanger.instance.ChangeScene(SceneManager.GetActiveScene().name, false);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
