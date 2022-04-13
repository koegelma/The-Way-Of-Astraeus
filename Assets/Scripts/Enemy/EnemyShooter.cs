using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform firePosition;
    [SerializeField] private float timeBetweenShots;
    private float shootCountdown;
    private Transform player;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        shootCountdown = timeBetweenShots;
    }
    private void Update()
    {
        LookAtPlayer();

        if (shootCountdown <= 0f)
        {
            Shoot();
            shootCountdown = timeBetweenShots;
            return;
        }
        shootCountdown -= Time.deltaTime;
    }

    private void LookAtPlayer()
    {
        transform.LookAt(player);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    private void Shoot()
    {
        Instantiate(projectile, firePosition.position, transform.rotation);
    }
}
