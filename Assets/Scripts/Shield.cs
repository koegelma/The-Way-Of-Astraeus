using System.Collections.Generic;
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
    private bool hasEMP;
    public GameObject burstEffectGO;
    private ParticleSystem burstEffect;

    private void Start()
    {
        if (GetComponent<PlayerStats>()) GetShieldValues();
        health = maxHealth;
        cooldown = cooldownTime;
        shield = Instantiate(shieldPrefab, transform.position, Quaternion.identity);
        shield.transform.SetParent(gameObject.transform);

        shield.GetComponent<Renderer>().sharedMaterial = new Material(shield.GetComponent<Renderer>().sharedMaterial);
        shieldMat = shield.GetComponent<Renderer>().sharedMaterial;
        maxOpacity = shieldMat.GetFloat("_Opacity");
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

    public void GetShieldValues()
    {
        PlayerStats playerStats = PlayerStats.instance;
        maxHealth = playerStats.shieldAmount;
        cooldownTime = playerStats.shieldCooldown;
        if (playerStats.Shield > 5) hasEMP = true;
    }

    private void SetOpacity()
    {
        float low = health;
        if (low < 15) low = 15;
        opacity = (low / maxHealth) * maxOpacity;
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
            if (hasEMP) SendEMP();
        }
        SetToCooldown();
    }

    private void SendEMP()
    {
        GameObject burst = Instantiate(burstEffectGO, transform.position, transform.rotation);
        burst.transform.SetParent(gameObject.transform);
        Destroy(burst, 3);

        List<Transform> collidedEnemies = new List<Transform>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, 200);
        foreach (Collider collider in colliders)
        {
            if (collider.GetComponentInParent<EnemyHealth>())
            {
                Transform rootTransform = collider.transform.root;
                if (!collidedEnemies.Contains(rootTransform)) collidedEnemies.Add(rootTransform);
            }
        }

        foreach (Transform enemy in collidedEnemies)
        {
            StartCoroutine(enemy.GetComponent<EnemyMovement>().HndEMP(3));
        }
    }

    private void SetToCooldown()
    {
        if (recharging) recharging = false;
        onCooldown = true;
        cooldown = cooldownTime;
    }
}
