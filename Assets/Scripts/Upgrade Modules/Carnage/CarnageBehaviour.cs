using UnityEngine;
[CreateAssetMenu(menuName = "Upgrade Module Behaviour/Carnage")]
public class CarnageBehaviour : UpgradeModuleBehaviour
{
    PlayerStats playerStats;
    public override void Apply(int _newStage)
    {
        playerStats = PlayerStats.instance;
        switch (_newStage)
        {
            case 1:
                playerStats.carnageTime = 3;
                playerStats.carnageDMG = 0.5f;
                playerStats.carnageMaxStacks = 1;
                break;
            case 2:
                playerStats.carnageTime = 3;
                playerStats.carnageDMG = 0.5f;
                playerStats.carnageMaxStacks = 2;
                break;
            case 3:
                playerStats.carnageTime = 5;
                playerStats.carnageDMG = 0.5f;
                playerStats.carnageMaxStacks = 2;
                break;
            case 4:
                playerStats.carnageTime = 5;
                playerStats.carnageDMG = 0.5f;
                playerStats.carnageMaxStacks = 3;
                break;
            case 5:
                playerStats.carnageTime = 7;
                playerStats.carnageDMG = 0.5f;
                playerStats.carnageMaxStacks = 3;
                break;
            case 6:
                playerStats.carnageTime = 7;
                playerStats.carnageDMG = 0.75f;
                playerStats.carnageMaxStacks = 5;
                break;
        }
    }
}
