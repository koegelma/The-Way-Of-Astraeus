using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject explosion;
    private float speed;
    private float damageAmount;
    private float distanceTravelled;
    private Vector3 lastPosition;
    private bool isEnemyProjectile;
    private bool hasHit;

    private void Start()
    {
        distanceTravelled = 0;
        lastPosition = transform.position;
    }

    private void Update()
    {
        if (hasHit) return;
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
        if ((isEnemyProjectile && collObj.GetComponentInParent<PlayerHealth>()) || (!isEnemyProjectile && collObj.GetComponentInParent<EnemyHealth>())) Damage(collObj.gameObject);
    }

    private void Damage(GameObject target)
    {
        hasHit = true;
        GameObject explosionEffect = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(explosionEffect, 2f);
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.enabled = false;

        if (target.GetComponentInParent<PlayerHealth>()) target.GetComponentInParent<PlayerHealth>().SubtractHealth(damageAmount);
        if (target.GetComponentInParent<EnemyHealth>()) target.GetComponentInParent<EnemyHealth>().SubtractHealth(damageAmount);

        Destroy(gameObject, 2f);
    }
}
