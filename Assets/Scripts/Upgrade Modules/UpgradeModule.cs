using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade Module", menuName = "Upgrade Module")]
public class UpgradeModule : ScriptableObject
{
    public new string name;
    public string[] descriptions;
    public int stage;

    public string GetDescription()
    {
        stage = PlayerStats.instance.GetCurrentStage(this.name);
        return descriptions[stage];
    }
}
