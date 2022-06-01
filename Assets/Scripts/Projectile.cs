using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private GameManager gameManager;
    private ObjectPooler objectPooler;
    private PlayerStats playerStats;
    [SerializeField] private float lifeTimeSeconds;
    [SerializeField] private bool isSecondaryProjectile;
    private float speed;
    private float damageAmount;
    private float aoeRadius;
    private bool isEnemyProjectile;
    private GameObject shooter;
    private PoolTag hitExplosion;


    private void Start()
    {
        gameManager = GameManager.instance;
        objectPooler = ObjectPooler.instance;
        playerStats = PlayerStats.instance;
    }

    private void OnEnable()
    {
        StartCoroutine(DisableGameObject(lifeTimeSeconds));
    }


    private void Update()
    {
        Translate();
    }

    public void SetProjectileValues(float _speed, float _damage, float _aoeRadius, GameObject _shooter)
    {
        speed = _speed;
        damageAmount = _damage;
        aoeRadius = _aoeRadius;
        shooter = _shooter;

        if (isSecondaryProjectile) hitExplosion = PoolTag.LARGEHITEXPLOSION;
        else hitExplosion = PoolTag.SMALLHITEXPLOSION;
    }

    public void SetToEnemyProjectile()
    {
        isEnemyProjectile = true;
        transform.localRotation = Quaternion.Euler(0, 180, 0);
    }

    private void Translate()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider _collObj)
    {
        if (gameManager.gameEnded) return;

        // ----- DAMAGE PLAYER BEHAVIOUR -----
        if (isEnemyProjectile && _collObj.GetComponentInParent<PlayerHealth>())
        {
            DamagePlayer(_collObj.transform);
            return;
        }

        // ----- DAMAGE ENEMY BEHAVIOUR -----
        if (!isEnemyProjectile && _collObj.GetComponentInParent<EnemyHealth>())
        {
            if (aoeRadius > 0)
            {
                HndSecondary();
                return;
            }

            if (playerStats.MassDestruction > 0)
            {
                HndMassDestruction(_collObj);
                return;
            }
            DamageEnemy(_collObj.transform);
            gameObject.SetActive(false);
        }
    }

    // ----- DAMAGE PLAYER BEHAVIOUR -----
    private void DamagePlayer(Transform _target)
    {
        bool shieldActive = false;
        Shield shield = null;
        if (playerStats.Shield > 0)
        {
            shield = _target.GetComponentInParent<Shield>();
            shieldActive = shield.ShieldActive;
        }

        if (playerStats.Armor > 0 && !shieldActive)
        {
            damageAmount -= playerStats.armorAmount;
            if (playerStats.Armor > 5 && shooter != null) ReflectDamage(shooter.transform, damageAmount * playerStats.armorReflAmount);
        }

        objectPooler.SpawnFromPool(hitExplosion.ToString(), _target.position, Quaternion.identity);
        GameObject damageUIGameObject = objectPooler.SpawnFromPool(PoolTag.DAMAGEUI.ToString(), _target.position, Quaternion.identity);
        DamageUI damageUI = damageUIGameObject.GetComponent<DamageUI>();
        damageUI.SetDamageAmount(damageAmount);

        if (shieldActive)
        {
            shield.SubtractHealth(damageAmount);
            damageUI.SetColor(Color.cyan);
        }
        else
        {
            _target.GetComponentInParent<PlayerHealth>().SubtractHealth(damageAmount);
            damageUI.SetColor(Color.red);
        }
        gameObject.SetActive(false);
    }

    private void ReflectDamage(Transform _target, float _amount)
    {
        if (_target == null) return;
        GameObject damageUIGameObject = objectPooler.SpawnFromPool(PoolTag.DAMAGEUI.ToString(), _target.position, Quaternion.identity);
        DamageUI damageUI = damageUIGameObject.GetComponent<DamageUI>();
        damageUI.SetDamageAmount(_amount);
        damageUI.SetColor(Color.white);

        if (_target.GetComponentInParent<Shield>())
        {
            if (_target.GetComponentInParent<Shield>().ShieldActive)
            {
                _target.GetComponentInParent<Shield>().SubtractHealth(_amount);
                return;
            }
        }
        _target.GetComponentInParent<EnemyHealth>().SubtractHealth(_amount);
    }

    // ----- DAMAGE ENEMY BEHAVIOUR -----
    private void DamageEnemy(Transform _target)
    {
        float currentDamage = damageAmount;
        if (playerStats.carnageActive) currentDamage += playerStats.carnageDMG * currentDamage * playerStats.carnageCurrStacks;

        objectPooler.SpawnFromPool(hitExplosion.ToString(), _target.position, Quaternion.identity);
        GameObject damageUIGameObject = objectPooler.SpawnFromPool(PoolTag.DAMAGEUI.ToString(), _target.position, Quaternion.identity);
        DamageUI damageUI = damageUIGameObject.GetComponent<DamageUI>();
        damageUI.SetDamageAmount(currentDamage);

        bool isCrit = false;
        if (playerStats.CritHit > 0)
        {
            if (!isSecondaryProjectile || playerStats.CritHit > 5)
            {
                if (IsLucky(playerStats.critChance))
                {
                    currentDamage += playerStats.critDMG * currentDamage;
                    damageUI.SetDamageAmount(currentDamage);
                    isCrit = true;
                    damageUI.SetColor(Color.magenta);
                }
            }
        }
        if (playerStats.Leech > 0)
        {
            if (IsLucky(playerStats.leechChance)) GameManager.instance.playerShip.GetComponent<PlayerHealth>().AddHealth(currentDamage * playerStats.leechAmount);
        }
        playerStats.totalDamage += currentDamage;

        if (_target.GetComponentInParent<Shield>())
        {
            if (_target.GetComponentInParent<Shield>().ShieldActive)
            {
                _target.GetComponentInParent<Shield>().SubtractHealth(currentDamage);
                if (!isCrit) damageUI.SetColor(Color.cyan);
                return;
            }
        }
        _target.GetComponentInParent<EnemyHealth>().SubtractHealth(currentDamage);
        if (!isCrit) damageUI.SetColor(Color.yellow);
    }

    private void HndSecondary()
    {
        List<Transform> collidedEnemies = new List<Transform>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, aoeRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.GetComponentInParent<EnemyHealth>())
            {
                Transform rootTransform = collider.transform.root;
                if (!collidedEnemies.Contains(rootTransform)) collidedEnemies.Add(rootTransform);
            }
        }
        foreach (Transform target in collidedEnemies)
        {
            DamageEnemy(target);
        }
        gameObject.SetActive(false);
    }

    private void HndMassDestruction(Collider _collObj)
    {
        Transform initialRootTransform = _collObj.transform.root;
        List<Transform> collidedEnemies = new List<Transform>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, playerStats.aoeRange);
        foreach (Collider collider in colliders)
        {
            if (collider.GetComponentInParent<EnemyHealth>())
            {
                Transform rootTransform = collider.transform.root;
                if (!collidedEnemies.Contains(rootTransform) && rootTransform != initialRootTransform) collidedEnemies.Add(rootTransform);
            }
        }

        if (playerStats.MassDestruction > 5 && collidedEnemies.Count > 0) collidedEnemies.Add(_collObj.transform);
        else DamageEnemy(_collObj.transform);

        damageAmount *= playerStats.aoeDMG;

        foreach (Transform target in collidedEnemies)
        {
            DamageEnemy(target);
        }
        gameObject.SetActive(false);
    }

    private bool IsLucky(float _chance)
    {
        if (Random.value <= _chance) return true;
        return false;
    }

    private IEnumerator DisableGameObject(float _seconds)
    {
        yield return new WaitForSeconds(_seconds);
        gameObject.SetActive(false);
    }
}
