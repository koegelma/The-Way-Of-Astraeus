using UnityEngine;
using System;
using System.Collections;

public class PlayerStats : MonoBehaviour, ISaveable
{
    private PlayerHealth playerHealth;
    private PlayerShooter playerShooter;
    public static PlayerStats instance;
    public CurrencyUI currencyUI;
    public int cAmount;
    public int sAmount;
    public bool isBallistic;
    public bool hasShield;
    public bool hasArmor;

    public float totalDamage;

    // SHIP UPGRADE MODULE STAGES (0: not active - 6 fully upgraded)
    private int projectileMadness;
    public int ProjectileMadness { get { return projectileMadness; } }
    private int critHit;
    public int CritHit { get { return critHit; } }
    private int carnage;
    public int Carnage { get { return carnage; } }
    private int splash;
    public int Splash { get { return splash; } }
    private int leech;
    public int Leech { get { return leech; } }

    // UPGRADE VARS
    public int projectileAmount;
    public bool secHasMultipleProjectiles;
    public float critChance;
    public float critDMG;
    public bool secHasCrit;
    public float leechChance;
    public float leechAmount;

    public bool hasLoaded = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerShooter = GetComponent<PlayerShooter>();
    }

    public IEnumerator InitializeNewRun()
    {
        yield return new WaitUntil(() => hasLoaded);

        cAmount = 0;
        hasShield = false;
        hasArmor = false;
        totalDamage = 0;
        projectileMadness = 0;
        critHit = 0;
        carnage = 0;
        splash = 0;
        leech = 0;
        projectileAmount = 1;
        secHasMultipleProjectiles = false;
        critChance = 0;
        critDMG = 0;
        secHasCrit = false;
        leechChance = 0;
        leechAmount = 0;

        playerHealth.maxHealth = 100;
        playerHealth.health = playerHealth.maxHealth;

        playerShooter.secMaxAmmo = 6;
        playerShooter.secCurAmmo = playerShooter.secMaxAmmo;
    }

    public void AddCoins(int _amount)
    {
        cAmount += _amount;
        currencyUI.UpdateCoinText();
    }

    public void AddSkillCurr(int _amount)
    {
        sAmount += _amount;
        currencyUI.UpdateSkillCurrText();
    }

    public int GetCurrentStage(string _name)
    {
        int stage = 0;
        switch (_name)
        {
            case "PROJECTILE MADNESS":
                stage = projectileMadness;
                break;
            case "CRITICAL DAMAGE":
                stage = critHit;
                break;
            case "CARNAGE":
                stage = carnage;
                break;
            case "SPLASH":
                stage = splash;
                break;
            case "LEECH":
                stage = leech;
                break;
        }
        return stage;
    }

    public int IncrementStage(string _name)
    {
        int newStage = -1;
        switch (_name)
        {
            case "PROJECTILE MADNESS":
                projectileMadness++;
                newStage = projectileMadness;
                break;
            case "CRITICAL DAMAGE":
                critHit++;
                newStage = critHit;
                break;
            case "CARNAGE":
                carnage++;
                newStage = carnage;
                break;
            case "SPLASH":
                splash++;
                newStage = splash;
                break;
            case "LEECH":
                leech++;
                newStage = leech;
                break;
        }
        return newStage;
    }

    public object SaveState()
    {
        return new SaveData()
        {
            cAmount = this.cAmount,
            sAmount = this.sAmount,
            hasShield = this.hasShield,
            hasArmor = this.hasArmor,
            isBallistic = this.isBallistic,

            totalDamage = this.totalDamage,

            projectileMadness = this.projectileMadness,
            critHit = this.critHit,
            carnage = this.carnage,
            splash = this.splash,
            leech = this.leech,

            projectileAmount = this.projectileAmount,
            secHasMultipleProjectiles = this.secHasMultipleProjectiles,
            critChance = this.critChance,
            critDMG = this.critDMG,
            secHasCrit = this.secHasCrit,
            leechChance = this.leechChance,
            leechAmount = this.leechAmount
        };
    }

    public void LoadState(object state)
    {
        var saveData = (SaveData)state;

        cAmount = saveData.cAmount;
        sAmount = saveData.sAmount;

        hasShield = saveData.hasShield;
        hasArmor = saveData.hasArmor;
        isBallistic = saveData.isBallistic;

        totalDamage = saveData.totalDamage;

        projectileMadness = saveData.projectileMadness;
        critHit = saveData.critHit;
        carnage = saveData.carnage;
        splash = saveData.splash;
        leech = saveData.leech;

        projectileAmount = saveData.projectileAmount;
        secHasMultipleProjectiles = saveData.secHasMultipleProjectiles;
        critChance = saveData.critChance;
        critDMG = saveData.critDMG;
        secHasCrit = saveData.secHasCrit;
        leechChance = saveData.leechChance;
        leechAmount = saveData.leechAmount;

        hasLoaded = true;
    }

    [Serializable]
    private struct SaveData
    {
        public int cAmount;
        public int sAmount;

        public bool hasShield;
        public bool hasArmor;
        public bool isBallistic;

        public float totalDamage;

        public int projectileMadness;
        public int critHit;
        public int carnage;
        public int splash;
        public int leech;

        public int projectileAmount;
        public bool secHasMultipleProjectiles;
        public float critChance;
        public float critDMG;
        public bool secHasCrit;
        public float leechChance;
        public float leechAmount;
    }
}
