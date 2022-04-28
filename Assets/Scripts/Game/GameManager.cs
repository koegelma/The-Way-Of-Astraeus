using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private GameObject gameOverUI;
    private float totalDamage;
    public float TotalDamage { get { return totalDamage; } }
    public static GameManager instance;
    public bool gameEnded;

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
        if (playerHealth.Health <= 0) StartCoroutine(EndGame());
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(1);
        gameEnded = true;
        gameOverUI.SetActive(true);
    }

    public void AddTotalDamage(float _damage)
    {
        totalDamage += _damage;
    }
}
