using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade Module", menuName = "Upgrade Module")]
public class UpgradeModule : ScriptableObject
{
    public new string name;
    public string[] descriptions;
    public int stage;
    public UpgradeModuleBehaviour upgradeModuleBehaviour;

    public string GetDescription()
    {
        stage = PlayerStats.instance.GetCurrentStage(this.name);
        return descriptions[stage];
    }

    public int Apply()
    {
        int newStage = PlayerStats.instance.IncrementStage(this.name);
        if (newStage == -1)
        {
            Debug.Log("Something went wrong when incrementing stage (return -1).");
            return newStage;
        }
        stage = newStage;
        upgradeModuleBehaviour.Apply(stage);
        return stage;
    }
}