using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class PlayerShooter : MonoBehaviour, ISaveable
{
    public SoundManager soundManager;
    private ObjectPooler objectPooler;
    private PlayerStats playerStats;
    private GameManager gameManager;

    [Header("Primary Weapon")]
    [SerializeField] private PoolTag primProjectile;
    [SerializeField] private float primProjectileSpeed;
    [SerializeField] private float primProjectileDamage;
    [SerializeField] private float primFireRate;
    private float primFireRateCurr;
    private float primFireCountdown = 0;
    private bool autoShoot = true;
    private string primProjectilePool;
    private float assaultCooldown = 0;
    private Coroutine assaultCoroutine;
    private float polarizeCooldown = 0;
    private Coroutine polarizeCoroutine;
    public GameObject laserPrefab;
    [SerializeField] private Image circularCooldown;

    [Header("Secondary Weapon")]
    [SerializeField] private PoolTag secProjectile;
    [SerializeField] private float secProjectileSpeed;
    [SerializeField] private float secProjectileDamage;
    [SerializeField] private float secFireRate;
    [SerializeField] private Transform secFirePosition;
    [SerializeField] private float aoeRadius;
    private float secFireCountdown = 0;
    private string secProjectilePool;
    public int secMaxAmmo = 6;
    public int secCurAmmo = 6;
    [SerializeField] private Text secMaxAmmoText;
    [SerializeField] private Text secCurAmmoText;
    [SerializeField] private Text weaponTypeText;
    [SerializeField] private Text abilityText;

    [Header("General Weapon Setup")]
    [SerializeField] private Transform leftFirePosition;
    [SerializeField] private ParticleSystem leftShootingParticle;
    [SerializeField] private Transform rightFirePosition;
    [SerializeField] private ParticleSystem rightShootingParticle;

    private void Start()
    {
        playerStats = PlayerStats.instance;
        gameManager = GameManager.instance;

        if (playerStats.isBallistic)
        {
            primProjectile = PoolTag.BULLETPROJECTILE;
            secProjectile = PoolTag.MISSILEPROJECTILE;
            weaponTypeText.text = "MISSILE";
        }
        else
        {
            primProjectile = PoolTag.LASERPROJECTILE;
            secProjectile = PoolTag.PLASMAPROJECTILE;
            weaponTypeText.text = "PLASMA";
        }

        objectPooler = ObjectPooler.instance;
        primProjectilePool = gameObject.name + primProjectile;
        secProjectilePool = gameObject.name + secProjectile;

        float primProjectiles = 100;
        if (playerStats.ProjectileMadness + playerStats.Assault > 8) primProjectiles *= 2;
        objectPooler.AllocateObjectPool(primProjectilePool, primProjectile, Mathf.RoundToInt(primProjectiles));
        objectPooler.AllocateObjectPool(secProjectilePool, secProjectile, Mathf.RoundToInt(20));//primFireRate * 5)); // adjust when multiple locations implemented, or fireRate changes over time
        objectPooler.AllocateObjectPool(PoolTag.DAMAGEUI.ToString(), PoolTag.DAMAGEUI, Mathf.RoundToInt(50)); // properly adjust to include possible hits from enemies -> move to objPooler?

        secCurAmmoText.text = secCurAmmo.ToString();
        secMaxAmmoText.text = " | " + secMaxAmmo;

        primFireRateCurr = primFireRate;

        circularCooldown.fillAmount = 0;
    }

    private void Update()
    {
        if (gameManager.gameEnded)
        {
            autoShoot = false;
            return;
        }
        if (Input.GetKeyDown(KeyCode.Q)) ToggleAutoShoot();

        if (playerStats.Assault > 0) HndAssault();
        if (playerStats.Polarize > 0) HndPolarize();

        HndSecInput();

        if (polarizeCoroutine != null) return;

        HndPrimInput();
    }

    // ----- PRIMARY -----
    private void HndPrimInput()
    {
        if (primFireCountdown <= 0)
        {
            if (autoShoot)
            {
                if (playerStats.projectileAmount > 1) ShootMultiplePrimProjectiles();
                else
                {
                    GameObject leftProjectile = objectPooler.SpawnFromPool(primProjectilePool, leftFirePosition.position, Quaternion.identity);
                    leftProjectile.GetComponent<Projectile>().SetProjectileValues(primProjectileSpeed, primProjectileDamage, 0, gameObject);

                    GameObject rightProjectile = objectPooler.SpawnFromPool(primProjectilePool, rightFirePosition.position, Quaternion.identity);
                    rightProjectile.GetComponent<Projectile>().SetProjectileValues(primProjectileSpeed, primProjectileDamage, 0, gameObject);
                }
                leftShootingParticle.Play();
                rightShootingParticle.Play();

                primFireCountdown = 1 / primFireRateCurr;
            }
            return;
        }
        primFireCountdown -= Time.deltaTime;
    }

    private void ShootMultiplePrimProjectiles()
    {
        float yRotation = 0;
        for (int i = 0; i < playerStats.projectileAmount; i++)
        {
            GameObject leftProjectile = objectPooler.SpawnFromPool(primProjectilePool, leftFirePosition.position, Quaternion.Euler(0, -yRotation, 0));
            leftProjectile.GetComponent<Projectile>().SetProjectileValues(primProjectileSpeed, primProjectileDamage, 0, gameObject);

            GameObject rightProjectile = objectPooler.SpawnFromPool(primProjectilePool, rightFirePosition.position, Quaternion.Euler(0, yRotation, 0));
            rightProjectile.GetComponent<Projectile>().SetProjectileValues(primProjectileSpeed, primProjectileDamage, 0, gameObject);
            yRotation += 5f;
        }
    }

    private void ToggleAutoShoot()
    {
        autoShoot = !autoShoot;
    }

    // ----- SECONDARY -----
    private void HndSecInput()
    {
        if (secFireCountdown <= 0)
        {
            if (Input.GetKey(KeyCode.Space) && secCurAmmo > 0)
            {
                if (playerStats.ProjectileMadness > 5) ShootMultipleSecProjectiles();
                else
                {
                    GameObject projectile = objectPooler.SpawnFromPool(secProjectilePool, secFirePosition.position, Quaternion.identity);
                    projectile.GetComponent<Projectile>().SetProjectileValues(secProjectileSpeed, secProjectileDamage, aoeRadius, gameObject);
                }

                secFireCountdown = 1 / secFireRate;
                secCurAmmo--;
                secCurAmmoText.text = secCurAmmo.ToString();
            }
            return;
        }
        secFireCountdown -= Time.deltaTime;
    }

    private void ShootMultipleSecProjectiles()
    {
        float yRotation = 0;
        for (int i = 0; i < playerStats.projectileAmount; i++)
        {
            GameObject projectile = objectPooler.SpawnFromPool(secProjectilePool, secFirePosition.position, Quaternion.Euler(0, yRotation, 0));
            projectile.GetComponent<Projectile>().SetProjectileValues(secProjectileSpeed, secProjectileDamage, aoeRadius, gameObject);

            if (yRotation > 0) yRotation += 5f;
            else yRotation -= 5f;
            yRotation = -yRotation;
        }
    }

    // ----- ASSAULT -----
    private void HndAssault()
    {
        if (assaultCooldown <= 0)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!autoShoot) ToggleAutoShoot();
                if (assaultCoroutine != null) StopCoroutine(assaultCoroutine);
                assaultCoroutine = StartCoroutine(ActivateAssault());
                assaultCooldown = Mathf.Infinity;
            }
        }
        assaultCooldown -= Time.deltaTime;
        UpdateAssaultCooldownUI();
    }

    private IEnumerator ActivateAssault()
    {
        primFireRateCurr = primFireRate * 2;
        abilityText.text = "ASSAULT";
        StartCoroutine(UpdateAbilityDurationUI(playerStats.assaultTime));
        yield return new WaitForSeconds(playerStats.assaultTime);
        primFireRateCurr = primFireRate;
        assaultCooldown = playerStats.assaultCooldown;
        assaultCoroutine = null;
    }

    private void UpdateAssaultCooldownUI()
    {
        if (assaultCoroutine != null) return;
        if (assaultCooldown <= 0)
        {
            circularCooldown.fillAmount = 0;
            abilityText.text = null;
        }
        if (assaultCooldown > 0)
        {
            circularCooldown.fillAmount = assaultCooldown / playerStats.assaultCooldown;
            abilityText.text = "ASSAULT";
        }
    }

    // ----- POLARIZE -----
    private void HndPolarize()
    {
        if (polarizeCooldown <= 0)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!autoShoot) ToggleAutoShoot();
                if (polarizeCoroutine != null) StopCoroutine(polarizeCoroutine);
                polarizeCoroutine = StartCoroutine(ActivatePolarize());
                polarizeCooldown = Mathf.Infinity;
            }
        }
        polarizeCooldown -= Time.deltaTime;
        UpdatePolarizeCooldownUI();
    }

    private IEnumerator ActivatePolarize()
    {
        GameObject newLaserRight = Instantiate(laserPrefab, rightFirePosition.position, Quaternion.identity);
        newLaserRight.GetComponent<PolarizeLaser>().SetLaserValues(primProjectileDamage);
        newLaserRight.transform.SetParent(rightFirePosition);
        var mainRight = rightShootingParticle.main;
        mainRight.loop = true;
        rightShootingParticle.Play();

        GameObject newLaserLeft = Instantiate(laserPrefab, leftFirePosition.position, Quaternion.identity);
        newLaserLeft.GetComponent<PolarizeLaser>().SetLaserValues(primProjectileDamage);
        newLaserLeft.transform.SetParent(leftFirePosition);
        var mainLeft = leftShootingParticle.main;
        mainLeft.loop = true;
        leftShootingParticle.Play();

        abilityText.text = "POLARIZE";
        StartCoroutine(UpdateAbilityDurationUI(playerStats.polarizeTime));
        yield return new WaitForSeconds(playerStats.polarizeTime);

        Destroy(newLaserRight);
        Destroy(newLaserLeft);

        mainRight.loop = false;
        mainLeft.loop = false;

        polarizeCooldown = playerStats.polarizeCooldown;
        polarizeCoroutine = null;
    }

    private void UpdatePolarizeCooldownUI()
    {
        if (polarizeCoroutine != null) return;
        if (polarizeCooldown <= 0)
        {
            circularCooldown.fillAmount = 0;
            abilityText.text = null;
        }
        if (polarizeCooldown > 0)
        {
            circularCooldown.fillAmount = polarizeCooldown / playerStats.polarizeCooldown;
            abilityText.text = "POLARIZE";
        }
    }

    private IEnumerator UpdateAbilityDurationUI(float _duration)
    {
        float t = 0;

        while (t < _duration)
        {
            t += Time.deltaTime;
            circularCooldown.fillAmount = t / _duration;
            yield return null;
        }
        circularCooldown.fillAmount = 1;
        //abilityText.text = null;
    }


    public void AddAmmo(float _amount)
    {
        if (secCurAmmo + _amount <= secMaxAmmo) secCurAmmo += (int)_amount;
        else secCurAmmo = secMaxAmmo;
        secCurAmmoText.text = secCurAmmo.ToString();
        soundManager.PlayAmmoPickup();
    }

    // ----- SAVE LOAD SYSTEM ------
    public object SaveState()
    {
        return new SaveData()
        {
            secMaxAmmo = this.secMaxAmmo,
            secCurAmmo = this.secCurAmmo
        };
    }

    public void LoadState(object state)
    {
        var saveData = (SaveData)state;
        secMaxAmmo = saveData.secMaxAmmo;
        secCurAmmo = saveData.secCurAmmo;
    }

    [Serializable]
    private struct SaveData
    {
        public int secMaxAmmo;
        public int secCurAmmo;
    }
}
