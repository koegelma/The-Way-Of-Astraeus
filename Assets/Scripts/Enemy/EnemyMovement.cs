using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    //private float t = 0;
    //private float newMoveSpeed;
    private Transform target;
    private int waypointIndex = 0;
    private MeshCollider boundsColl;


    private void Start()
    {
        target = EnemyWaypoints.waypoints[0];
        boundsColl = Bounds.bounds.GetComponent<MeshCollider>();
    }

    private void Update()
    {
        //Vector3 currTarget = target.position;
        if (Vector3.Distance(transform.position, target.position) <= 2.5f) GetNextWaypoint();

        //float currMoveSpeed = moveSpeed;
        //float newMoveSpeed = moveSpeed;
        //if (!boundsColl.bounds.Contains(new Vector3(transform.position.x, boundsColl.transform.position.y, transform.position.z))) newMoveSpeed *= 1.2f;

        //currMoveSpeed = Mathf.Lerp(currMoveSpeed, newMoveSpeed, t);
        //t += 0.5f;


        transform.position = Vector3.Lerp(transform.position, target.position, moveSpeed * Time.deltaTime);
        //Vector3 direction = target.position - transform.position;
        //transform.Translate(direction.normalized * currMoveSpeed * Time.deltaTime, Space.World);
    }

    private void GetNextWaypoint()
    {
        /* if (waypointIndex >= EnemyWaypoints.waypoints.Length - 1)
        {
            waypointIndex = 0;
            return;
        }
        waypointIndex++; */
        waypointIndex = Random.Range(0, EnemyWaypoints.waypoints.Length);
        target = EnemyWaypoints.waypoints[waypointIndex];
        //t = 0;
        //newMoveSpeed = moveSpeed * 1.5f;
    }
}
