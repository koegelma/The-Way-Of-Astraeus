using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/CoinPickup")]
public class CoinPickup : PowerupEffect
{
    public int amount; // for coins with different amounts; delete otherwise
    public override void Apply(GameObject target)
    {
        PlayerStats.instance.AddCoins(amount);
    }
}
