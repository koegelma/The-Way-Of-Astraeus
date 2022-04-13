using UnityEngine;

public class Powerup : MonoBehaviour
{
    public PowerupEffect powerupEffect;
    private void OnTriggerEnter(Collider collObj)
    {
        Destroy(gameObject);
        powerupEffect.Apply(collObj.gameObject);
    }
}
