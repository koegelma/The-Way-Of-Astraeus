using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float startHealth;
    [SerializeField] private ParticleSystem addHealthEffect;
    private float health;
    public float StartHealth { get { return startHealth; } }
    public float Health { get { return health; } }

    private void Start()
    {
        health = startHealth;
    }

    private void Update()
    {
        if (health <= 0) Die();
    }

    public void AddHealth(float _amount)
    {
        if (health + _amount <= startHealth) health += _amount;
        else health = startHealth;
        addHealthEffect.Play();
    }

    public void SubtractHealth(float _amount)
    {
        if (health - _amount >= 0) health -= _amount;
        else health = 0;
    }

    private void Die()
    {
        ObjectPooler.instance.SpawnFromPool(PoolTag.SHIPEXPLOSION.ToString(), transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

}
