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

    private void Start()
    {
        objectPooler = ObjectPooler.instance;
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
            Damage(_collObj.transform);
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
                AoEDamage(collidedEnemies);
                return;
            }
            Damage(_collObj.transform);
        }
    }

    private void Damage(Transform _target)
    {
        objectPooler.SpawnFromPool(hitExplosion.ToString(), _target.position, Quaternion.identity);
        GameObject damageUIGameObject = objectPooler.SpawnFromPool(PoolTag.DAMAGEUI.ToString(), _target.position, Quaternion.identity);
        DamageUI damageUI = damageUIGameObject.GetComponent<DamageUI>();
        damageUI.SetDamageAmount(damageAmount);

        if (_target.GetComponentInParent<PlayerHealth>())
        {
            _target.GetComponentInParent<PlayerHealth>().SubtractHealth(damageAmount);
            damageUI.SetColor(Color.red);
            gameObject.SetActive(false);
            return;
        }

        if (_target.GetComponentInParent<EnemyHealth>())
        {
            GameManager.instance.AddTotalDamage(damageAmount);
            if (_target.GetComponentInParent<Shield>())
            {
                if (_target.GetComponentInParent<Shield>().ShieldActive)
                {
                    _target.GetComponentInParent<Shield>().SubtractHealth(damageAmount);
                    damageUI.SetColor(Color.cyan);
                    gameObject.SetActive(false);
                    return;
                }
            }
            _target.GetComponentInParent<EnemyHealth>().SubtractHealth(damageAmount);
            damageUI.SetColor(Color.yellow);
            gameObject.SetActive(false);
        }
    }

    private void AoEDamage(List<Transform> _targets)
    {
        foreach (Transform target in _targets)
        {
            GameManager.instance.AddTotalDamage(damageAmount);
            objectPooler.SpawnFromPool(hitExplosion.ToString(), target.position, Quaternion.identity);
            GameObject damageUIGameObject = objectPooler.SpawnFromPool(PoolTag.DAMAGEUI.ToString(), target.position, Quaternion.identity);
            DamageUI damageUI = damageUIGameObject.GetComponent<DamageUI>();
            damageUI.SetDamageAmount(damageAmount);

            if (target.GetComponentInParent<Shield>())
            {
                if (target.GetComponentInParent<Shield>().ShieldActive)
                {
                    target.GetComponentInParent<Shield>().SubtractHealth(damageAmount);
                    damageUI.SetColor(Color.cyan);
                    return;
                }
            }

            if (target.GetComponentInParent<EnemyHealth>())
            {
                target.GetComponentInParent<EnemyHealth>().SubtractHealth(damageAmount);
                damageUI.SetColor(Color.yellow);
            }
        }
        gameObject.SetActive(false);
    }

    private IEnumerator DisableGameObject(float _seconds)
    {
        yield return new WaitForSeconds(_seconds);
        gameObject.SetActive(false);
    }
}
