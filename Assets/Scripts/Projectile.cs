using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifeTimeSeconds;
    private float speed;
    private float damageAmount;
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

    public void SetProjectileValues(float _speed, float _damage)
    {
        speed = _speed;
        damageAmount = _damage;

        if (damageAmount >= 20) hitExplosion = PoolTag.LARGEHITEXPLOSION;
        else hitExplosion = PoolTag.SMALLHITEXPLOSION;
    }

    public void SetToEnemyProjectile()
    {
        isEnemyProjectile = true;
    }

    private void Translate()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider _collObj)
    {
        if ((isEnemyProjectile && _collObj.GetComponentInParent<PlayerHealth>()) || (!isEnemyProjectile && _collObj.GetComponentInParent<EnemyHealth>())) Damage(_collObj.gameObject);
    }

    private void Damage(GameObject _target)
    {
        objectPooler.SpawnFromPool(hitExplosion.ToString(), transform.position, Quaternion.identity);
        GameObject damageUIGameObject = objectPooler.SpawnFromPool(PoolTag.DAMAGEUI.ToString(), transform.position, Quaternion.identity);
        DamageUI damageUI = damageUIGameObject.GetComponent<DamageUI>();
        damageUI.SetDamageAmount(damageAmount);

        if (_target.GetComponentInParent<PlayerHealth>())
        {
            _target.GetComponentInParent<PlayerHealth>().SubtractHealth(damageAmount);
            damageUI.SetColor(Color.red);
        }
        if (_target.GetComponentInParent<EnemyHealth>())
        {
            _target.GetComponentInParent<EnemyHealth>().SubtractHealth(damageAmount);
            damageUI.SetColor(Color.yellow);
            GameManager.instance.AddTotalDamage(damageAmount);
        }
        gameObject.SetActive(false);
    }

    private IEnumerator DisableGameObject(float _seconds)
    {
        yield return new WaitForSeconds(_seconds);
        gameObject.SetActive(false);
    }
}
