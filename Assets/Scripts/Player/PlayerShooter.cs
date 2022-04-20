using System;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileDamage;
    [SerializeField] private Transform firePosition;
    [SerializeField] private float fireRate;
    private float fireCountdown = 0;

    private void Update()
    {
        if (fireCountdown <= 0)
        {
            HndInput();
            return;
        }
        fireCountdown -= Time.deltaTime;
    }

    private void HndInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            GameObject newProjectile = Instantiate(projectile, firePosition.position, Quaternion.identity);
            newProjectile.GetComponent<Projectile>().SetProjectileValues(projectileSpeed, projectileDamage);
            fireCountdown = 1 / fireRate;
        }
    }
}
