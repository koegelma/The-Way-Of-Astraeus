using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float startHealth;
    private float health;

    private void Start()
    {
        health = startHealth;
    }

    private void Update()
    {
        if (health <= 0) Die();
    }

    public void SubtractHealth(float _amount)
    {
        if (health - _amount >= 0) health -= _amount;
        else health = 0;
        //Debug.Log(gameObject.name + " health = " + health);
    }

    private void Die()
    {
        ObjectPooler.instance.SpawnFromPool(PoolTag.SHIPEXPLOSION.ToString(), transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
