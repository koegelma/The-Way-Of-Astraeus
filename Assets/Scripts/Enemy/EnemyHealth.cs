using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float startHealth;
    private float health;
    [SerializeField] private float smallHealthChance; // 0 - 1.0
    [SerializeField] private GameObject smallHealthBuff;

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
        if (IsLucky(smallHealthChance)) Instantiate(smallHealthBuff, transform.position, Quaternion.identity);
        ObjectPooler.instance.SpawnFromPool(PoolTag.SHIPEXPLOSION.ToString(), transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private bool IsLucky(float chance)
    {
        if (Random.value <= chance) return true;
        return false;
    }
}
