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
    private bool autoShoot = false;
    private ObjectPooler objectPooler;
    private string projectilePool;

    private void Start()
    {
        objectPooler = ObjectPooler.instance;
        projectilePool = gameObject.name + projectile;

        objectPooler.AllocateObjectPool(projectilePool, projectile, Mathf.RoundToInt(fireRate * 5)); // adjust when multiple locations implemented, or fireRate changes over time
        objectPooler.AllocateObjectPool(PoolTag.DAMAGEUI.ToString(), PoolTag.DAMAGEUI, Mathf.RoundToInt(fireRate * 5)); // properly adjust to include possible hits from enemies
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)) ToggleAutoShoot();
        if (fireCountdown <= 0)
        {
            HndInput();
            return;
        }
        fireCountdown -= Time.deltaTime;
    }

    private void HndInput()
    {
        if (Input.GetKey(KeyCode.Space) || autoShoot) //|| Input.GetMouseButton(0))
        {
            //GameObject leftProjectile = Instantiate(projectile, leftFirePosition.position, Quaternion.identity);
            GameObject leftProjectile = objectPooler.SpawnFromPool(projectilePool, leftFirePosition.position, Quaternion.identity);
            leftProjectile.GetComponent<Projectile>().SetProjectileValues(projectileSpeed, projectileDamage);
            leftShootingParticle.Play();
            //GameObject rightProjectile = Instantiate(projectile, rightFirePosition.position, Quaternion.identity);
            GameObject rightProjectile = objectPooler.SpawnFromPool(projectilePool, rightFirePosition.position, Quaternion.identity);
            rightProjectile.GetComponent<Projectile>().SetProjectileValues(projectileSpeed, projectileDamage);
            rightShootingParticle.Play();
            fireCountdown = 1 / fireRate;
        }
    }

    private void ToggleAutoShoot()
    {
        autoShoot = !autoShoot;
    }
}
