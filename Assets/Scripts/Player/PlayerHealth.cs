using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, ISaveable
{
    [SerializeField] private float maxHealth;
    [SerializeField] private ParticleSystem addHealthEffect;
    private float health;
    public float MaxHealth { get { return maxHealth; } }
    public float Health { get { return health; } }

    private void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        if (health <= 0) Die();
    }

    public void AddHealth(float _amount)
    {
        if (health + _amount <= maxHealth) health += _amount;
        else health = maxHealth;
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

    // SAVE LOAD SYSTEM
    public object SaveState()
    {
        return new SaveData()
        {
            maxHealth = this.maxHealth
        };
    }

    public void LoadState(object state)
    {
        var saveData = (SaveData)state;
        maxHealth = saveData.maxHealth;
    }

    [Serializable]
    private struct SaveData
    {
        public float maxHealth;
    }

}
