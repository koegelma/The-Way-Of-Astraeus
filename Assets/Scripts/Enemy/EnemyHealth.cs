using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerStats playerStats;
    [SerializeField] private float startHealth;
    private float health;
    [SerializeField] private GameObject smallHealthBuff;
    [SerializeField] private GameObject ammoPickup;
    [SerializeField] private int cWorth;

    private void Start()
    {
        health = startHealth;
        playerStats = PlayerStats.instance;
        gameManager = GameManager.instance;
    }

    private void Update()
    {
        if (gameManager.gameEnded) gameObject.SetActive(false);
        if (health <= 0) Die();
    }

    public void SubtractHealth(float _amount)
    {
        if (health - _amount >= 0) health -= _amount;
        else health = 0;
    }

    private void Die()
    {
        Vector3 pickupSpawnPosition = transform.position;
        if (IsLucky(playerStats.healthDropChance))
        {
            Instantiate(smallHealthBuff, pickupSpawnPosition, Quaternion.identity);
            pickupSpawnPosition.x += 2;
        }
        if (IsLucky(playerStats.ammoDropChance)) Instantiate(ammoPickup, pickupSpawnPosition, Quaternion.identity);

        int cAmount = Mathf.RoundToInt(cWorth * playerStats.cDropMultiplier);
        for (int i = 0; i < cAmount; i++)
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
