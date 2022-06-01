using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private EnemyShooter enemyShooter;
    [SerializeField] private float moveSpeed;
    private Transform target;
    private int waypointIndex = 0;
    private bool isStunned;
    public GameObject stunEffect;
    public Transform stunPos;

    private void Start()
    {
        target = EnemyWaypoints.waypoints[Random.Range(0, EnemyWaypoints.waypoints.Length)];
        if (GetComponent<EnemyShooter>()) enemyShooter = GetComponent<EnemyShooter>();
    }

    private void Update()
    {
        if (isStunned) return;
        if (Vector3.Distance(transform.position, target.position) <= 2.5f) GetNextWaypoint();

        transform.position = Vector3.Lerp(transform.position, target.position, moveSpeed * Time.deltaTime);
    }

    private void GetNextWaypoint()
    {
        waypointIndex = Random.Range(0, EnemyWaypoints.waypoints.Length);
        target = EnemyWaypoints.waypoints[waypointIndex];
    }

    public IEnumerator HndEMP(float _stunTime)
    {
        yield return new WaitForSeconds(1);
        isStunned = true;
        if (enemyShooter != null) enemyShooter.isStunned = true;
        if (stunPos == null || transform == null) yield break;
        GameObject stunEffectGO = Instantiate(stunEffect, stunPos.position, transform.rotation);
        yield return new WaitForSeconds(_stunTime);
        isStunned = false;
        if (enemyShooter != null) enemyShooter.isStunned = false;
        Destroy(stunEffectGO);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
