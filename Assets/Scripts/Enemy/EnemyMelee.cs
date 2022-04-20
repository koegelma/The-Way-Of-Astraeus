using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    [SerializeField] private float speed;
    private Transform player;
    private void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        LookAtPlayer();
        Translate();
    }

    private void LookAtPlayer()
    {
        transform.LookAt(player);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    private void Translate()
    {
        if (Vector3.Distance(transform.position, player.position) < 1.2f)
        {
            transform.Translate(Vector3.back * speed * 50 * Time.deltaTime);
            return;
        }

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
