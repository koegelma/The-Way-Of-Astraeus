using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image background;
    private bool healthBarEnabled;

    private void Start()
    {
        StartCoroutine(DisableHealthBar(0));
    }

    private void Update()
    {
        if (playerHealth.GetHealth() >= playerHealth.GetStartHealth())
        {
            if (healthBarEnabled) StartCoroutine(DisableHealthBar(3));
            return;
        }
        else if (!healthBarEnabled) EnableHealthbar();

        healthBar.fillAmount = playerHealth.GetHealth() / playerHealth.GetStartHealth();
    }

    private void EnableHealthbar()
    {
        healthBarEnabled = true;
        healthBar.enabled = true;
        background.enabled = true;
    }

    private IEnumerator DisableHealthBar(float _seconds)
    {
        healthBarEnabled = false;
        healthBar.fillAmount = playerHealth.GetHealth() / playerHealth.GetStartHealth();
        yield return new WaitForSeconds(_seconds);
        healthBar.enabled = false;
        background.enabled = false;
    }
}
