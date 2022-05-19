using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/AmmoPickup")]
public class AmmoPickup : PowerupEffect
{
    public float amount;
    public override void Apply(GameObject target)
    {
        target.GetComponentInParent<PlayerShooter>().AddAmmo(amount);
    }
}
