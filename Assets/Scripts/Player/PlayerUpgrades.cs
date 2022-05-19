using UnityEngine;
using System;

public class PlayerUpgrades : MonoBehaviour, ISaveable
{
    /* private bool hasShield;
    public bool HasShield { get { return hasShield; } }
    private bool hasArmor;
    public bool HasArmor { get { return hasArmor; } }
    private bool isBallistic;
    public bool IsBallistic { get { return isBallistic; } } */
    public static PlayerUpgrades instance;
    public bool hasShield;
    public bool hasArmor;
    public bool isBallistic;

    private void Awake()
    {
        instance = this;
    }

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
