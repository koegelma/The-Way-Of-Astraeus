using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifeTimeSeconds;
    [SerializeField] private bool isSecondaryProjectile;
    private float speed;
    private float damageAmount;
    private float aoeRadius;
    private bool isEnemyProjectile;
    private PoolTag hitExplosion;
    private ObjectPooler objectPooler;
    private PlayerStats playerStats;

    private void Start()
    {
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

    public void SetProjectileValues(float _speed, float _damage, float _aoeRadius)
    {
        speed = _speed;
        damageAmount = _damage;
        aoeRadius = _aoeRadius;

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
        if (GameManager.instance.gameEnded) return;

        if (isEnemyProjectile && _collObj.GetComponentInParent<PlayerHealth>())
        {
            DamagePlayer(_collObj.transform);
            return;
        }

        if (!isEnemyProjectile && _collObj.GetComponentInParent<EnemyHealth>())
        {
            if (aoeRadius > 0)
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
                SecondaryAoEDamage(collidedEnemies);
                return;
            }

            if (playerStats.MassDestruction > 0)
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
                MassDestructionAoEDamage(_collObj.transform, collidedEnemies);
                return;
            }
            DamageEnemy(_collObj.transform);
            gameObject.SetActive(false);
        }
    }

    private void DamagePlayer(Transform _target)
    {
        objectPooler.SpawnFromPool(hitExplosion.ToString(), _target.position, Quaternion.identity);
        GameObject damageUIGameObject = objectPooler.SpawnFromPool(PoolTag.DAMAGEUI.ToString(), _target.position, Quaternion.identity);
        DamageUI damageUI = damageUIGameObject.GetComponent<DamageUI>();
        damageUI.SetDamageAmount(damageAmount);

        _target.GetComponentInParent<PlayerHealth>().SubtractHealth(damageAmount);
        damageUI.SetColor(Color.red);
        gameObject.SetActive(false);
    }

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
            if (!isSecondaryProjectile || playerStats.secHasCrit)
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

    private void MassDestructionAoEDamage(Transform _initialTarget, List<Transform> _targets)
    {
        if (playerStats.MassDestruction > 5 && _targets.Count > 0) _targets.Add(_initialTarget);
        else DamageEnemy(_initialTarget);

        damageAmount *= playerStats.aoeDMG;

        foreach (Transform target in _targets)
        {
            DamageEnemy(target);
        }

        gameObject.SetActive(false);
    }

    private void SecondaryAoEDamage(List<Transform> _targets)
    {
        foreach (Transform target in _targets)
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
