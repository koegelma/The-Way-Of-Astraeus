using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/CoinPickup")]
public class CoinPickup : PowerupEffect
{
    public int amount;
    public override void Apply(GameObject target)
    {
        target.GetComponent<PlayerStats>().AddCoins(amount);
    }
}
