using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image background;
    [SerializeField] private Text currentLife;
    [SerializeField] private Text maxLife;
    private bool healthBarEnabled;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(DisableHealthBar(0));
    }

    private void Update()
    {
        if (playerHealth.Health >= playerHealth.MaxHealth)
        {
            if (healthBarEnabled) StartCoroutine(DisableHealthBar(3));
            return;
        }
        else if (!healthBarEnabled)
        {
            StopAllCoroutines();
            EnableHealthbar();
        }

        healthBar.fillAmount = playerHealth.Health / playerHealth.MaxHealth;
        currentLife.text = playerHealth.Health.ToString() + " ";
        maxLife.text = "| " + playerHealth.MaxHealth.ToString();
    }

    private void EnableHealthbar()
    {
        healthBarEnabled = true;
        healthBar.enabled = true;
        background.enabled = true;
        currentLife.enabled = true;
        maxLife.enabled = true;
        animator.SetTrigger("TrFadeIn");
    }

    private IEnumerator DisableHealthBar(float _seconds)
    {
        healthBarEnabled = false;
        healthBar.fillAmount = playerHealth.Health / playerHealth.MaxHealth;
        currentLife.text = playerHealth.Health.ToString() + " ";
        maxLife.text = "| " + playerHealth.MaxHealth.ToString();
        animator.SetTrigger("TrFadeOut");
        yield return new WaitForSeconds(_seconds);
        healthBar.enabled = false;
        background.enabled = false;
        currentLife.enabled = false;
        maxLife.enabled = false;
    }
}
