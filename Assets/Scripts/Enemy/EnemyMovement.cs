using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private Rigidbody rb;
    private Vector3 position = Vector3.zero;
    private float changeDirectionTimer;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(position);
    }

    private void Update()
    {
        Vector3 movement = new Vector3(0, 0, 0);
        if (changeDirectionTimer <= 0)
        {
            //
            return;
        }
        changeDirectionTimer -= Time.deltaTime;

        position = rb.position + movement * moveSpeed * Time.fixedDeltaTime;
    }
}
