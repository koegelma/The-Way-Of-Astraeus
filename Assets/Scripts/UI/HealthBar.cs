using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image background;
    [SerializeField] private GameObject healthbar;
    [SerializeField] private Text currentLife;
    [SerializeField] private Text maxLife;
    private bool healthBarEnabled;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        healthbar.SetActive(false);
    }

    private void Update()
    {
        if (playerHealth.health >= playerHealth.maxHealth)
        {
            if (healthBarEnabled) StartCoroutine(DisableHealthBar(3));
            return;
        }
        else if (!healthBarEnabled)
        {
            StopAllCoroutines();
            EnableHealthbar();
        }
        healthBar.fillAmount = playerHealth.health / playerHealth.maxHealth;
        currentLife.text = Mathf.Round(playerHealth.health).ToString() + " ";
        maxLife.text = "| " + playerHealth.maxHealth.ToString();
    }

    private void EnableHealthbar()
    {
        healthBarEnabled = true;
        healthbar.SetActive(true);
        animator.SetTrigger("TrFadeIn");
    }

    private IEnumerator DisableHealthBar(float _seconds)
    {
        healthBarEnabled = false;
        healthBar.fillAmount = playerHealth.health / playerHealth.maxHealth;
        currentLife.text = playerHealth.health.ToString() + " ";
        maxLife.text = "| " + playerHealth.maxHealth.ToString();
        animator.SetTrigger("TrFadeOut");
        yield return new WaitForSeconds(_seconds);
        healthbar.SetActive(false);
    }
}
