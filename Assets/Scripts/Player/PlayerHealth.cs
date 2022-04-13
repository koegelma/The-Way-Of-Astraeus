using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float startHealth;
    private float health;

    private void Start()
    {
        health = startHealth;
    }

    public float GetHealth() { return health; }
    public float GetStartHealth() { return startHealth; }

    public void AddHealth(float amount)
    {
        if (health + amount <= startHealth) health += amount;
        else health = startHealth;
    }

    public void SubtractHealth(float amount)
    {
        if (health - amount >= 0) health -= amount;
        else health = 0;
    }
}
