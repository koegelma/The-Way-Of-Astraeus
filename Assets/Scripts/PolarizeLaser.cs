using UnityEngine;

public class PolarizeLaser : MonoBehaviour
{
    private PlayerStats playerStats;
    private ObjectPooler objectPooler;
    private GameManager gameManager;
    private LineRenderer lineRenderer;
    private float damageOverTime;
    private Vector3 targetPosition;
    private float maxLength = 200;
    private RaycastHit hit;
    private GameObject hitObject;
    private float accumulatedDMG = 0;
    private float dmgDisplayTime = 0.1f;
    private float nextDmgDisplayTime = 0;

    private void Start()
    {
        playerStats = PlayerStats.instance;
        objectPooler = ObjectPooler.instance;
        gameManager = GameManager.instance;
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        float distance = maxLength;

        if (EnemyHit())
        {
            // ----- TODO: Mass Destruction behaviour -----
            hitObject = hit.collider.gameObject;
            Damage();
            distance = Vector3.Distance(transform.position, hit.collider.transform.position);

            if (accumulatedDMG > 0)
            {
                if (nextDmgDisplayTime <= 0) DisplayDMG();
                nextDmgDisplayTime -= Time.deltaTime;
            }
        }

        targetPosition = transform.position + new Vector3(0, 0, distance);
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, targetPosition);
    }

    private bool EnemyHit()
    {
        Vector3 rayOrigin = new Vector3(transform.position.x, 0, transform.position.z);
        Ray fwdRay = new Ray(rayOrigin, Vector3.forward);
        Physics.Raycast(fwdRay, out hit);
        if (hit.collider.gameObject.GetComponentInParent<EnemyHealth>()) return true;
        return false;
    }

    private void Damage()
    {
        float currentDamage = damageOverTime * playerStats.projectileAmount * 2 * Time.deltaTime;
        if (playerStats.carnageActive) currentDamage += playerStats.carnageDMG * currentDamage * playerStats.carnageCurrStacks;

        if (playerStats.CritHit > 0) if (IsLucky(playerStats.critChance)) currentDamage += playerStats.critDMG * currentDamage;

        if (playerStats.Leech > 0) if (IsLucky(playerStats.leechChance)) gameManager.playerShip.GetComponent<PlayerHealth>().AddHealth(currentDamage * playerStats.leechAmount);

        playerStats.totalDamage += currentDamage;

        accumulatedDMG += currentDamage;

        if (hitObject.GetComponentInParent<Shield>())
        {
            if (hitObject.GetComponentInParent<Shield>().ShieldActive)
            {
                hitObject.GetComponentInParent<Shield>().SubtractHealth(currentDamage);
                return;
            }
        }
        hitObject.GetComponentInParent<EnemyHealth>().SubtractHealth(currentDamage);
    }

    private void DisplayDMG()
    {
        GameObject damageUIGameObject = objectPooler.SpawnFromPool(PoolTag.DAMAGEUI.ToString(), hitObject.transform.position, Quaternion.identity);
        DamageUI damageUI = damageUIGameObject.GetComponent<DamageUI>();
        damageUI.SetDamageAmount(accumulatedDMG);
        damageUI.SetColor(Color.yellow);

        accumulatedDMG = 0;
        nextDmgDisplayTime = dmgDisplayTime;
    }

    public void SetLaserValues(float _damageOverTime)
    {
        damageOverTime = _damageOverTime;
    }

    private bool IsLucky(float _chance)
    {
        if (Random.value <= _chance) return true;
        return false;
    }
}
