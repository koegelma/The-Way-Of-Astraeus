using UnityEngine;

public class Powerup : MonoBehaviour
{
    public PowerupEffect powerupEffect;
    private void OnTriggerEnter(Collider collObj)
    {
        if (collObj.GetComponent<PlayerMovement>())
        {
            Destroy(gameObject);
            powerupEffect.Apply(collObj.gameObject);
        }
    }
}
