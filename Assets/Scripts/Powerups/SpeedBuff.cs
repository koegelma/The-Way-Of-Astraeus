using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/SpeedBuff")]
public class SpeedBuff : PowerupEffect
{
    public float amount;
    public override void Apply(GameObject target)
    {
        target.GetComponentInParent<PlayerMovement>().moveSpeed += amount;
    }
}
