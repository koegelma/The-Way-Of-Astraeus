using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float startHealth;
    private float health;
    [SerializeField] private float smallHealthChance; // 0 - 1.0
    [SerializeField] private GameObject smallHealthBuff;
    [SerializeField] private float ammoPickupChance; // 0 - 1.0
    [SerializeField] private GameObject ammoPickup;
    [SerializeField] private int cWorth;
    private PlayerStats playerStats;

    private void Start()
    {
        health = startHealth;
        playerStats = PlayerStats.instance;
    }

    private void Update()
    {
        if (GameManager.instance.gameEnded) gameObject.SetActive(false);
        if (health <= 0) Die();
    }

    public void SubtractHealth(float _amount)
    {
        if (health - _amount >= 0) health -= _amount;
        else health = 0;
    }

    private void Die()
    {
        if (IsLucky(smallHealthChance)) Instantiate(smallHealthBuff, transform.position, Quaternion.identity);
        if (IsLucky(ammoPickupChance)) Instantiate(ammoPickup, transform.position, Quaternion.identity);

        for (int i = 0; i < cWorth; i++)
        {
            ObjectPooler.instance.SpawnFromPool(PoolTag.COIN.ToString(), transform.position, Quaternion.identity);
        }

        if (playerStats.Carnage > 0)
        {
            if (playerStats.carnageCurrStacks <= playerStats.carnageMaxStacks) playerStats.ActivateCarnageStack();
        }

        ObjectPooler.instance.SpawnFromPool(PoolTag.SHIPEXPLOSION.ToString(), transform.position, Quaternion.identity);
        EnemySpawner.enemiesToDie--;
        Destroy(gameObject);
    }

    private bool IsLucky(float _chance)
    {
        if (Random.value <= _chance) return true;
        return false;
    }
}
