using System;
using System.Collections;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [SerializeField] private PoolTag projectile;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileDamage;
    [SerializeField] private Transform firePosition;
    [SerializeField] private ParticleSystem shootingParticle;
    [SerializeField] private float fireRate;
    private float fireCountdown;
    private ObjectPooler objectPooler;
    private string projectilePool;
    private string m_ID = Guid.NewGuid().ToString();

    private void Start()
    {
        fireCountdown = 1 * UnityEngine.Random.Range(2, 4) / fireRate;
        objectPooler = ObjectPooler.instance;
        projectilePool = m_ID + projectile.ToString();
        objectPooler.AllocateObjectPool(projectilePool, projectile, Mathf.RoundToInt(fireRate * 6));
    }
    private void Update()
    {
        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1 / fireRate;
            return;
        }
        fireCountdown -= Time.deltaTime;
    }

    private void Shoot()
    {
        GameObject newProjectile = objectPooler.SpawnFromPool(projectilePool, firePosition.position, Quaternion.identity);
        newProjectile.GetComponent<Projectile>().SetToEnemyProjectile();
        newProjectile.GetComponent<Projectile>().SetProjectileValues(projectileSpeed, projectileDamage);
        shootingParticle.Play();
    }

    private void OnDestroy()
    {
        objectPooler.DelayDeallocatingObjectPool(projectilePool, 3);
    }
}
