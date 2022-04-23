using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private PoolTag projectile;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileDamage;
    [SerializeField] private Transform leftFirePosition;
    [SerializeField] private ParticleSystem leftShootingParticle;
    [SerializeField] private Transform rightFirePosition;
    [SerializeField] private ParticleSystem rightShootingParticle;
    [SerializeField] private float fireRate;
    private float fireCountdown = 0;
    private ObjectPooler objectPooler;

    private void Start()
    {
        objectPooler = ObjectPooler.instance;
        objectPooler.AllocateObjectPool(gameObject.name, projectile, 25);
    }

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
        if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
        {
            //GameObject leftProjectile = Instantiate(projectile, leftFirePosition.position, Quaternion.identity);
            GameObject leftProjectile = objectPooler.SpawnFromPool(gameObject.name, leftFirePosition.position, Quaternion.identity);
            leftProjectile.GetComponent<Projectile>().SetProjectileValues(projectileSpeed, projectileDamage);
            leftShootingParticle.Play();
            //GameObject rightProjectile = Instantiate(projectile, rightFirePosition.position, Quaternion.identity);
            GameObject rightProjectile = objectPooler.SpawnFromPool(gameObject.name, rightFirePosition.position, Quaternion.identity);
            rightProjectile.GetComponent<Projectile>().SetProjectileValues(projectileSpeed, projectileDamage);
            rightShootingParticle.Play();
            fireCountdown = 1 / fireRate;
        }
    }
}
