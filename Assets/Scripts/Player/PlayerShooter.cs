using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileDamage;
    [SerializeField] private Transform leftFirePosition;
    [SerializeField] private ParticleSystem leftShootingParticle;
    [SerializeField] private Transform rightFirePosition;
    [SerializeField] private ParticleSystem rightShootingParticle;
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
            GameObject leftProjectile = Instantiate(projectile, leftFirePosition.position, Quaternion.identity);
            leftProjectile.GetComponent<Projectile>().SetProjectileValues(projectileSpeed, projectileDamage);
            leftShootingParticle.Play();
            rightShootingParticle.Play();
            GameObject rightProjectile = Instantiate(projectile, rightFirePosition.position, Quaternion.identity);
            rightProjectile.GetComponent<Projectile>().SetProjectileValues(projectileSpeed, projectileDamage);
            fireCountdown = 1 / fireRate;
        }
    }
}
