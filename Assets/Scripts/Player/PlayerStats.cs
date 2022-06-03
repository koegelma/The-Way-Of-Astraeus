using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour, ISaveable
{
    [Header("Scripts")]
    private PlayerHealth playerHealth;
    private PlayerShooter playerShooter;
    public static PlayerStats instance;
    public CurrencyUI currencyUI;
    public SoundManager soundManager;

    [Header("General Vars")]
    public int cAmount;
    public int sAmount;
    public float cDropMultiplier;
    public bool isBallistic;
    public float totalDamage;
    public float healthDropChance;
    public float ammoDropChance;

    [Header("Ship Upgrade Module Stages (0: not active - 6 fully upgraded)")]
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
    private int shield;
    public int Shield { get { return shield; } }
    private int armor;
    public int Armor { get { return armor; } }
    private int polarize;
    public int Polarize { get { return polarize; } }
    private int assault;
    public int Assault { get { return assault; } }

    [Header("Ship Upgrade Module Vars")]
    public int projectileAmount;
    public float critChance;
    public float critDMG;
    public float aoeRange;
    public float aoeDMG;
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
    public float shieldAmount;
    public float shieldCooldown;
    public float armorAmount;
    public float armorReflAmount;
    public float polarizeTime;
    public float polarizeCooldown;
    public float assaultTime;
    public float assaultCooldown;

    private bool hasLoaded = false;

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
        cDropMultiplier = 1;
        totalDamage = 0;
        healthDropChance = 0.1f;
        ammoDropChance = 0.1f;

        projectileMadness = 0;
        critHit = 0;
        carnage = 0;
        massDestruction = 0;
        leech = 0;
        shield = 0;
        armor = 0;
        polarize = 0;
        assault = 0;

        projectileAmount = 1;
        critChance = 0;
        critDMG = 0;
        aoeRange = 0;
        aoeDMG = 0;
        leechChance = 0;
        leechAmount = 0;
        carnageDMG = 0;
        carnageTime = 0;
        carnageMaxStacks = 0;
        carnageCurrStacks = 0;
        carnageActive = false;
        armorAmount = 0;
        armorReflAmount = 0;
        polarizeTime = 0;
        polarizeCooldown = 0;
        assaultTime = 0;
        assaultCooldown = 0;

        playerHealth.maxHealth = 100;
        playerHealth.health = playerHealth.maxHealth;

        playerShooter.secMaxAmmo = 6;
        playerShooter.secCurAmmo = playerShooter.secMaxAmmo;
    }

    public void AddCoins(int _amount)
    {
        cAmount += _amount;
        currencyUI.UpdateCoinText();
        //StartCoroutine(soundManager.QueuePlayCoinPickup());
        soundManager.PlayCoinPickup();
    }

    public void AddSkillCurr(int _amount)
    {
        sAmount += _amount;
        currencyUI.UpdateSkillCurrText();
    }

    public int GetCurrentStage(string _moduleName)
    {
        int stage = 0;
        switch (_moduleName)
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
            case "SHIELD":
                stage = shield;
                break;
            case "ARMOR":
                stage = armor;
                break;
            case "POLARIZE":
                stage = polarize;
                break;
            case "ASSAULT":
                stage = assault;
                break;
        }
        return stage;
    }

    public int IncrementStage(string _moduleName)
    {
        int newStage = -1;
        switch (_moduleName)
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
            case "SHIELD":
                shield++;
                newStage = shield;
                break;
            case "ARMOR":
                armor++;
                newStage = armor;
                break;
            case "POLARIZE":
                polarize++;
                newStage = polarize;
                break;
            case "ASSAULT":
                assault++;
                newStage = assault;
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

    public void ActivateShield()
    {
        Shield shield = GetComponent<Shield>();
        shield.enabled = true;
    }

    // ----- SAVE LOAD SYSTEM ------
    public object SaveState()
    {
        return new SaveData()
        {
            cAmount = this.cAmount,
            sAmount = this.sAmount,
            cDropMultiplier = this.cDropMultiplier,
            isBallistic = this.isBallistic,
            totalDamage = this.totalDamage,
            healthDropChance = this.healthDropChance,
            ammoDropChance = this.ammoDropChance,

            projectileMadness = this.projectileMadness,
            critHit = this.critHit,
            carnage = this.carnage,
            splash = this.massDestruction,
            leech = this.leech,
            shield = this.shield,
            armor = this.armor,
            polarize = this.polarize,
            assault = this.assault,

            projectileAmount = this.projectileAmount,
            critChance = this.critChance,
            critDMG = this.critDMG,
            aoeRange = this.aoeRange,
            aoeDMG = this.aoeDMG,
            leechChance = this.leechChance,
            leechAmount = this.leechAmount,
            carnageDMG = this.carnageDMG,
            carnageTime = this.carnageTime,
            carnageMaxStacks = this.carnageMaxStacks,
            carnageCurrStacks = 0,
            carnageActive = false,
            shieldAmount = this.shieldAmount,
            shieldCooldown = this.shieldCooldown,
            armorAmount = this.armorAmount,
            armorReflAmount = this.armorReflAmount,
            polarizeTime = this.polarizeTime,
            polarizeCooldown = this.polarizeCooldown,
            assaultTime = this.assaultTime,
            assaultCooldown = this.assaultCooldown
        };
    }

    public void LoadState(object state)
    {
        var saveData = (SaveData)state;

        cAmount = saveData.cAmount;
        sAmount = saveData.sAmount;
        cDropMultiplier = saveData.cDropMultiplier;
        isBallistic = saveData.isBallistic;
        totalDamage = saveData.totalDamage;
        healthDropChance = saveData.healthDropChance;
        ammoDropChance = saveData.ammoDropChance;

        projectileMadness = saveData.projectileMadness;
        critHit = saveData.critHit;
        carnage = saveData.carnage;
        massDestruction = saveData.splash;
        leech = saveData.leech;
        shield = saveData.shield;
        armor = saveData.armor;
        polarize = saveData.polarize;
        assault = saveData.assault;

        projectileAmount = saveData.projectileAmount;
        critChance = saveData.critChance;
        critDMG = saveData.critDMG;
        aoeRange = saveData.aoeRange;
        aoeDMG = saveData.aoeDMG;
        leechChance = saveData.leechChance;
        leechAmount = saveData.leechAmount;
        carnageDMG = saveData.carnageDMG;
        carnageTime = saveData.carnageTime;
        carnageMaxStacks = saveData.carnageMaxStacks;
        carnageCurrStacks = saveData.carnageCurrStacks;
        carnageActive = saveData.carnageActive;
        shieldAmount = saveData.shieldAmount;
        shieldCooldown = saveData.shieldCooldown;
        armorAmount = saveData.armorAmount;
        armorReflAmount = saveData.armorReflAmount;
        polarizeTime = saveData.polarizeTime;
        polarizeCooldown = saveData.polarizeCooldown;
        assaultTime = saveData.assaultTime;
        assaultCooldown = saveData.assaultCooldown;

        hasLoaded = true;

        if (shield > 0) ActivateShield();
    }

    [Serializable]
    private struct SaveData
    {
        public int cAmount;
        public int sAmount;
        public float cDropMultiplier;
        public bool isBallistic;
        public float totalDamage;
        public float healthDropChance;
        public float ammoDropChance;

        public int projectileMadness;
        public int critHit;
        public int carnage;
        public int splash;
        public int leech;
        public int shield;
        public int armor;
        public int polarize;
        public int assault;

        public int projectileAmount;
        public float critChance;
        public float critDMG;
        public float aoeRange;
        public float aoeDMG;
        public float leechChance;
        public float leechAmount;
        public float carnageDMG;
        public float carnageTime;
        public int carnageMaxStacks;
        public int carnageCurrStacks;
        public bool carnageActive;
        public float shieldAmount;
        public float shieldCooldown;
        public float armorAmount;
        public float armorReflAmount;
        public float polarizeTime;
        public float polarizeCooldown;
        public float assaultTime;
        public float assaultCooldown;
    }
}
