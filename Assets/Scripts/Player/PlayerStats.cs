using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

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
    private int massDestruction;
    public int MassDestruction { get { return massDestruction; } }
    private int leech;
    public int Leech { get { return leech; } }

    // UPGRADE VARS
    public int projectileAmount;
    public bool secHasMultipleProjectiles;
    public float critChance;
    public float critDMG;
    public float aoeRange;
    public float aoeDMG;
    public bool secHasCrit;
    public float leechChance;
    public float leechAmount;
    public float carnageDMG;
    public float carnageTime;
    public int carnageMaxStacks;
    public int carnageCurrStacks;
    public bool carnageActive;
    private Queue<Coroutine> carnageCoroutines;
    public Text carnageStackText;
    public Text carnageText;

    public bool hasLoaded = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerShooter = GetComponent<PlayerShooter>();
        carnageCoroutines = new Queue<Coroutine>();
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
        massDestruction = 0;
        leech = 0;

        projectileAmount = 1;
        secHasMultipleProjectiles = false;
        critChance = 0;
        critDMG = 0;
        aoeRange = 0;
        aoeDMG = 0;
        secHasCrit = false;
        leechChance = 0;
        leechAmount = 0;
        carnageDMG = 0;
        carnageTime = 0;
        carnageMaxStacks = 0;
        carnageCurrStacks = 0;
        carnageActive = false;

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
            case "MASS DESTRUCTION":
                stage = massDestruction;
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
            case "MASS DESTRUCTION":
                massDestruction++;
                newStage = massDestruction;
                break;
            case "LEECH":
                leech++;
                newStage = leech;
                break;
        }
        return newStage;
    }

    public void ActivateCarnageStack()
    {
        if (carnageCurrStacks == carnageMaxStacks && carnageCoroutines.Count == carnageMaxStacks)
        {
            Coroutine coroutineToReset = carnageCoroutines.Dequeue();
            StopCoroutine(coroutineToReset);
            carnageCurrStacks--;
        }

        Coroutine newCarnageCoroutine = StartCoroutine(StartCarnageStack());
        carnageCoroutines.Enqueue(newCarnageCoroutine);
    }

    private IEnumerator StartCarnageStack()
    {
        carnageCurrStacks++;
        UpdateCarnageUI();
        carnageActive = true;

        yield return new WaitForSeconds(carnageTime);

        carnageCurrStacks--;
        UpdateCarnageUI();
        if (carnageCurrStacks < 1) carnageActive = false;
        carnageCoroutines.Dequeue();
    }

    private void UpdateCarnageUI()
    {
        if (carnageCurrStacks < 1) carnageText.text = null;
        else carnageText.text = "CARNAGE STACKS   ";
        carnageStackText.text = carnageCurrStacks.ToString("#");
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
            splash = this.massDestruction,
            leech = this.leech,

            projectileAmount = this.projectileAmount,
            secHasMultipleProjectiles = this.secHasMultipleProjectiles,
            critChance = this.critChance,
            critDMG = this.critDMG,
            aoeRange = this.aoeRange,
            aoeDMG = this.aoeDMG,
            secHasCrit = this.secHasCrit,
            leechChance = this.leechChance,
            leechAmount = this.leechAmount,
            carnageDMG = this.carnageDMG,
            carnageTime = this.carnageTime,
            carnageMaxStacks = this.carnageMaxStacks,
            carnageCurrStacks = 0,
            carnageActive = false
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
        massDestruction = saveData.splash;
        leech = saveData.leech;

        projectileAmount = saveData.projectileAmount;
        secHasMultipleProjectiles = saveData.secHasMultipleProjectiles;
        critChance = saveData.critChance;
        critDMG = saveData.critDMG;
        aoeRange = saveData.aoeRange;
        aoeDMG = saveData.aoeDMG;
        secHasCrit = saveData.secHasCrit;
        leechChance = saveData.leechChance;
        leechAmount = saveData.leechAmount;
        carnageDMG = saveData.carnageDMG;
        carnageTime = saveData.carnageTime;
        carnageMaxStacks = saveData.carnageMaxStacks;
        carnageCurrStacks = saveData.carnageCurrStacks;
        carnageActive = saveData.carnageActive;

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
        public float aoeRange;
        public float aoeDMG;
        public bool secHasCrit;
        public float leechChance;
        public float leechAmount;
        public float carnageDMG;
        public float carnageTime;
        public int carnageMaxStacks;
        public int carnageCurrStacks;
        public bool carnageActive;
    }
}
