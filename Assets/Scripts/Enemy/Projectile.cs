using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float damageAmount;
    private Transform player;
    private float distanceTravelled;
    private Vector3 lastPosition;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        distanceTravelled = 0f;
        lastPosition = transform.position;
    }

    private void Update()
    {
        Translate();
        if (distanceTravelled > 100f) Destroy(gameObject);
    }

    private void Translate()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        distanceTravelled += Vector3.Distance(transform.position, lastPosition);
        lastPosition = transform.position;
    }

    private void OnTriggerEnter(Collider collObj)
    {
        if (collObj.transform == player) Damage();
    }

    private void Damage()
    {
        player.GetComponent<PlayerHealth>().SubtractHealth(damageAmount);
        Destroy(gameObject);
    }
}
