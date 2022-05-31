using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade Module Behaviour/Mass Destruction")]
public class MassDestructionBehaviour : UpgradeModuleBehaviour
{
    PlayerStats playerStats;
    public override void Apply(int _newStage)
    {
        playerStats = PlayerStats.instance;
        switch (_newStage)
        {
            case 1:
                playerStats.aoeRange = 10;
                playerStats.aoeDMG = 0.5f;
                break;
            case 2:
                playerStats.aoeRange = 15;
                playerStats.aoeDMG = 0.5f;
                break;
            case 3:
                playerStats.aoeRange = 15;
                playerStats.aoeDMG = 0.75f;
                break;
            case 4:
                playerStats.aoeRange = 20;
                playerStats.aoeDMG = 0.75f;
                break;
            case 5:
                playerStats.aoeRange = 25;
                playerStats.aoeDMG = 1;
                break;
            case 6:
                playerStats.aoeRange = 25;
                playerStats.aoeDMG = 1.5f;
                break;
        }
    }
}
