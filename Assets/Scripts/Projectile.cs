using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed;
    private float damageAmount;
    private float distanceTravelled;
    private Vector3 lastPosition;
    private bool isEnemyProjectile;

    private void Start()
    {
        distanceTravelled = 0;
        lastPosition = transform.position;
    }

    private void Update()
    {
        Translate();
        if (distanceTravelled > 500) Destroy(gameObject);
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
        distanceTravelled += Vector3.Distance(transform.position, lastPosition);
        lastPosition = transform.position;
    }

    private void OnTriggerEnter(Collider collObj)
    {
        if ((isEnemyProjectile && collObj.GetComponentInParent<PlayerHealth>()) || (!isEnemyProjectile && collObj.GetComponent<EnemyShooter>())) Damage(collObj.gameObject);
    }

    private void Damage(GameObject target)
    {
        if (target.GetComponentInParent<PlayerHealth>()) target.GetComponentInParent<PlayerHealth>().SubtractHealth(damageAmount);
        if (target.GetComponent<EnemyHealth>()) target.GetComponent<EnemyHealth>().SubtractHealth(damageAmount);
        Destroy(gameObject);
    }
}
