using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private float totalDamage;
    public bool gameEnded;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private GameObject gameOverUI;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        gameEnded = false;
        totalDamage = 0;
    }

    private void Update()
    {
        if (playerHealth.GetHealth() <= 0) StartCoroutine(EndGame());
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(1);
        gameEnded = true;
        gameOverUI.SetActive(true);
    }

    public float GetTotalDamage()
    {
        return totalDamage;
    }

    public void AddTotalDamage(float _damage)
    {
        totalDamage += _damage;
    }
}
