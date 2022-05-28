using UnityEngine;
using UnityEngine.UI;

public class PlayerShooter : MonoBehaviour
{
    [Header("Primary Weapon")]
    [SerializeField] private PoolTag primProjectile;
    [SerializeField] private float primProjectileSpeed;
    [SerializeField] private float primProjectileDamage;
    [SerializeField] private float primFireRate;
    private float primFireCountdown = 0;
    private bool autoShoot = true;
    private string primProjectilePool;
    private int projectileAmount = 5;

    [Header("Secondary Weapon")]
    [SerializeField] private PoolTag secProjectile;
    [SerializeField] private float secProjectileSpeed;
    [SerializeField] private float secProjectileDamage;
    [SerializeField] private float secFireRate;
    [SerializeField] private Transform secFirePosition;
    [SerializeField] private float aoeRadius;
    private float secFireCountdown = 0;
    private string secProjectilePool;
    private int secMaxAmmo = 6; // needs to be saved in Save/Load system
    private int secCurAmmo; // needs to be saved in Save/Load system
    [SerializeField] private Text secMaxAmmoText;
    [SerializeField] private Text secCurAmmoText;
    [SerializeField] private AudioSource reloadSound;


    [Header("General Setup")]
    [SerializeField] private Transform leftFirePosition;
    [SerializeField] private ParticleSystem leftShootingParticle;
    [SerializeField] private Transform rightFirePosition;
    [SerializeField] private ParticleSystem rightShootingParticle;
    private ObjectPooler objectPooler;

    private void Start()
    {
        if (PlayerStats.instance.isBallistic)
        {
            primProjectile = PoolTag.BULLETPROJECTILE;
            secProjectile = PoolTag.MISSILEPROJECTILE;
        }
        else
        {
            primProjectile = PoolTag.LASERPROJECTILE;
            secProjectile = PoolTag.PLASMAPROJECTILE;
        }

        objectPooler = ObjectPooler.instance;
        primProjectilePool = gameObject.name + primProjectile;
        secProjectilePool = gameObject.name + secProjectile;

        objectPooler.AllocateObjectPool(primProjectilePool, primProjectile, Mathf.RoundToInt(100));//primFireRate * 5)); // adjust when multiple locations implemented, or fireRate changes over time
        objectPooler.AllocateObjectPool(secProjectilePool, secProjectile, Mathf.RoundToInt(10));//primFireRate * 5)); // adjust when multiple locations implemented, or fireRate changes over time
        objectPooler.AllocateObjectPool(PoolTag.DAMAGEUI.ToString(), PoolTag.DAMAGEUI, Mathf.RoundToInt(primFireRate * 5)); // properly adjust to include possible hits from enemies -> move to objPooler?

        secCurAmmo = secMaxAmmo;
        secCurAmmoText.text = secCurAmmo.ToString();
        secMaxAmmoText.text = " | " + secMaxAmmo;
    }

    private void Update()
    {
        if (GameManager.instance.gameEnded)
        {
            autoShoot = false;
            return;
        }
        if (Input.GetKeyDown(KeyCode.E)) ToggleAutoShoot();
        HndPrimInput();
        HndSecInput();
    }

    private void HndPrimInput()
    {
        if (primFireCountdown <= 0)
        {
            if (autoShoot) //|| Input.GetMouseButton(0))
            {
                for (int i = 0; i < projectileAmount; i++)
                {
                    GameObject leftProjectile = objectPooler.SpawnFromPool(primProjectilePool, leftFirePosition.position, Quaternion.identity);
                    leftProjectile.GetComponent<Projectile>().SetProjectileValues(primProjectileSpeed, primProjectileDamage, 0);
                    leftShootingParticle.Play();

                    GameObject rightProjectile = objectPooler.SpawnFromPool(primProjectilePool, rightFirePosition.position, Quaternion.identity);
                    rightProjectile.GetComponent<Projectile>().SetProjectileValues(primProjectileSpeed, primProjectileDamage, 0);
                    rightShootingParticle.Play();
                }


                primFireCountdown = 1 / primFireRate;
            }
            return;
        }
        primFireCountdown -= Time.deltaTime;
    }

    private void HndSecInput()
    {
        if (secFireCountdown <= 0)
        {
            if (Input.GetKey(KeyCode.Space) && secCurAmmo > 0)
            {
                GameObject projectile = objectPooler.SpawnFromPool(secProjectilePool, secFirePosition.position, Quaternion.identity);
                projectile.GetComponent<Projectile>().SetProjectileValues(secProjectileSpeed, secProjectileDamage, aoeRadius);

                secFireCountdown = 1 / secFireRate;
                secCurAmmo--;
                secCurAmmoText.text = secCurAmmo.ToString();
            }
            return;
        }
        secFireCountdown -= Time.deltaTime;
    }

    private void ToggleAutoShoot()
    {
        autoShoot = !autoShoot;
    }

    public void AddAmmo(float _amount)
    {
        if (secCurAmmo + _amount <= secMaxAmmo) secCurAmmo += (int)_amount;
        else secCurAmmo = secMaxAmmo;
        secCurAmmoText.text = secCurAmmo.ToString();
        reloadSound.Play();
    }
}
