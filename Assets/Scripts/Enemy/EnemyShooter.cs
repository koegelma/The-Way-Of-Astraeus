using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileDamage;
    [SerializeField] private Transform firePosition;
    [SerializeField] private ParticleSystem shootingParticle;
    [SerializeField] private float fireRate;
    private float fireCountdown;
    //private Transform player;

    private void Start()
    {
        //player = GameObject.Find("PlayerShip").transform;
        fireCountdown = fireRate * Random.Range(2, 4);
    }
    private void Update()
    {
        if (fireCountdown <= 0f)
        //&& Vector3.Distance(player.position, transform.position) < 70)
        {
            Shoot();
            fireCountdown = 1 / fireRate;
            return;
        }
        fireCountdown -= Time.deltaTime;
    }

    private void Shoot()
    {
        GameObject newProjectile = Instantiate(projectile, firePosition.position, transform.rotation); // TODO: get from obj pool
        newProjectile.GetComponent<Projectile>().SetToEnemyProjectile();
        newProjectile.GetComponent<Projectile>().SetProjectileValues(projectileSpeed, projectileDamage);
        shootingParticle.Play();
    }
}
