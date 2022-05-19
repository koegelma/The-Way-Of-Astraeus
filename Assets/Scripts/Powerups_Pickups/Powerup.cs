using UnityEngine;

public class Powerup : MonoBehaviour
{
    public PowerupEffect powerupEffect;
    private void OnTriggerEnter(Collider collObj)
    {
        if (collObj.gameObject.GetComponentInParent<PlayerHealth>())
        {
            Destroy(gameObject);
            powerupEffect.Apply(collObj.gameObject);
        }
    }

    private void Update()
    {
        transform.position += Vector3.back * 20 * Time.deltaTime;
    }
}
