using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour, ISaveable
{
    /* private bool hasShield;
    public bool HasShield { get { return hasShield; } }
    private bool hasArmor;
    public bool HasArmor { get { return hasArmor; } }
    private bool isBallistic;
    public bool IsBallistic { get { return isBallistic; } } */
    public static PlayerStats instance;
    public CurrencyUI currencyUI;
    public int cAmount;
    public int sAmount;
    public bool isBallistic;

    // SHIP UPGRADE MODULE BOOLS
    public bool hasProjectileMadness;
    public bool hasCritHit;
    public bool hasCarnage;
    public bool hasSplash;
    public bool hasLeech;

    public bool hasShield;
    public bool hasArmor;

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

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        cAmount = 0;
        sAmount = 10000;
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
            case "CritHit":
                stage = critHit;
                break;
            case "Carnage":
                stage = carnage;
                break;
            case "Splash":
                stage = splash;
                break;
            case "Leech":
                stage = leech;
                break;
        }
        return stage;
    }

    /* public void IncrementStage(string _name)
    {
        switch (_name)
        {
            case "PROJECTILE MADNESS":
                if (!hasProjectileMadness && projectileMadness == 0)
                {
                    hasProjectileMadness = true;
                    projectileMadness++;
                }
                if
                break;
            case "CritHit":
                stage = critHit;
                break;
            case "Carnage":
                stage = carnage;
                break;
            case "Splash":
                stage = splash;
                break;
            case "Leech":
                stage = leech;
                break;
        }
    } */

    /*  private int GetIncrement(int _currentStage)
     {

     } */

    public object SaveState()
    {
        return new SaveData()
        {
            hasShield = this.hasShield,
            hasArmor = this.hasArmor,
            isBallistic = this.isBallistic
        };
    }

    public void LoadState(object state)
    {
        var saveData = (SaveData)state;
        hasShield = saveData.hasShield;
        hasArmor = saveData.hasArmor;
        isBallistic = saveData.isBallistic;
    }

    [Serializable]
    private struct SaveData
    {
        public bool hasShield;
        public bool hasArmor;
        public bool isBallistic;
    }
}
