using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private GameObject shieldPrefab;
    private GameObject shield;
    [SerializeField] private float maxHealth;
    private float health;
    [SerializeField] private float rechargeTime;
    [SerializeField] private float cooldownTime;
    private float cooldown;
    private bool onCooldown;
    private bool recharging;
    private Material shieldMat;
    private float opacity;
    private float maxOpacity;
    public bool ShieldActive { get { return shield.activeSelf; } }

    private void Start()
    {
        health = maxHealth;
        shield = Instantiate(shieldPrefab, transform.position, Quaternion.identity);
        shield.transform.SetParent(gameObject.transform);

        shield.GetComponent<Renderer>().sharedMaterial = new Material(shield.GetComponent<Renderer>().sharedMaterial);
        shieldMat = shield.GetComponent<Renderer>().sharedMaterial;
        maxOpacity = shieldMat.GetFloat("_Opacity");

        cooldown = cooldownTime;
    }

    private void Update()
    {
        SetOpacity();

        if (onCooldown)
        {
            HndCooldown();
            return;
        }
        if (recharging) Recharge();
    }

    private void SetOpacity()
    {
        opacity = (health / maxHealth) * maxOpacity;
        shieldMat.SetFloat("_Opacity", opacity);
    }

    private void HndCooldown()
    {
        if (cooldown <= 0)
        {
            shield.SetActive(true);
            cooldown = cooldownTime;
            onCooldown = false;
            recharging = true;
            return;
        }
        cooldown -= Time.deltaTime;
    }

    private void Recharge()
    {
        health += Time.deltaTime * maxHealth / rechargeTime;
        if (health >= maxHealth)
        {
            health = maxHealth;
            recharging = false;
        }
    }

    public void SubtractHealth(float _amount)
    {
        if (health - _amount > 0) health -= _amount;
        else
        {
            health = 0;
            shield.SetActive(false);
        }
        SetToCooldown();
    }

    private void SetToCooldown()
    {
        if (recharging) recharging = false;
        onCooldown = true;
        cooldown = cooldownTime;
    }
}
