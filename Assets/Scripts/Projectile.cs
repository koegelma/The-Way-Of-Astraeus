using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifeTimeSeconds;
    private float speed;
    private float damageAmount;
    private bool isEnemyProjectile;
   
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
        ObjectPooler.instance.SpawnFromPool(PoolTag.HITEXPLOSION.ToString(), transform.position, Quaternion.identity);

        if (_target.GetComponentInParent<PlayerHealth>()) _target.GetComponentInParent<PlayerHealth>().SubtractHealth(damageAmount);
        if (_target.GetComponentInParent<EnemyHealth>()) _target.GetComponentInParent<EnemyHealth>().SubtractHealth(damageAmount);
        gameObject.SetActive(false);
    }

    private IEnumerator DisableGameObject(float _seconds)
    {
        yield return new WaitForSeconds(_seconds);
        gameObject.SetActive(false);
    }
}
