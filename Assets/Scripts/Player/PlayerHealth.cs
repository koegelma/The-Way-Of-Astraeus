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

    public void AddHealth(float _amount)
    {
        if (health + _amount <= startHealth) health += _amount;
        else health = startHealth;
    }

    public void SubtractHealth(float _amount)
    {
        if (health - _amount >= 0) health -= _amount;
        else health = 0;
    }
}
