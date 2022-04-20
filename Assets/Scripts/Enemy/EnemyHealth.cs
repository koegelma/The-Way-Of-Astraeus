using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float startHealth;
    private float health;

    private void Start()
    {
        health = startHealth;
    }

    private void Update()
    {
        if (health <= 0) Destroy(gameObject);
    }

    public void SubtractHealth(float amount)
    {
        if (health - amount >= 0) health -= amount;
        else health = 0;
        Debug.Log(gameObject.name + " health = " + health);
    }
}
