using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private Transform target;
    private int waypointIndex = 0;
    private MeshCollider boundsColl;


    private void Start()
    {
        target = EnemyWaypoints.waypoints[Random.Range(0, EnemyWaypoints.waypoints.Length)];
        boundsColl = Bounds.bounds.GetComponent<MeshCollider>();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, target.position) <= 2.5f) GetNextWaypoint();

        transform.position = Vector3.Lerp(transform.position, target.position, moveSpeed * Time.deltaTime);
    }

    private void GetNextWaypoint()
    {
        waypointIndex = Random.Range(0, EnemyWaypoints.waypoints.Length);
        target = EnemyWaypoints.waypoints[waypointIndex];
    }
}
