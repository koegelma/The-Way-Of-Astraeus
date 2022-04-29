using UnityEngine;
using System;

public class PlayerUpgrades : MonoBehaviour, ISaveable
{
    private bool hasShield;
    public bool HasShield { get { return hasShield; } }

    public object SaveState()
    {
        return new SaveData()
        {
            hasShield = this.hasShield
        };
    }

    public void LoadState(object state)
    {
        var saveData = (SaveData)state;
        hasShield = saveData.hasShield;
    }

    [Serializable]
    private struct SaveData
    {
        public bool hasShield;
    }
}
