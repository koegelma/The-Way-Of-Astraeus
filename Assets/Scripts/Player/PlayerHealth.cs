using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, ISaveable
{
    public ParticleSystem addHealthEffect;
    public float maxHealth = 100;
    public float health = 100;

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

     // ----- SAVE LOAD SYSTEM ------
    public object SaveState()
    {
        return new SaveData()
        {
            maxHealth = this.maxHealth,
            health = this.health
        };
    }

    public void LoadState(object state)
    {
        var saveData = (SaveData)state;
        maxHealth = saveData.maxHealth;
        health = saveData.health;
    }

    [Serializable]
    private struct SaveData
    {
        public float maxHealth;
        public float health;
    }

}
