using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Image healthBar;

    private void Update()
    {
        healthBar.fillAmount = playerHealth.GetHealth() / playerHealth.GetStartHealth();
    }
}
