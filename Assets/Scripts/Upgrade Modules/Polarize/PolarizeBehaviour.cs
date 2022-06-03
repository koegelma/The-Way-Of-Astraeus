using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade Module Behaviour/Polarize")]
public class PolarizeBehaviour : UpgradeModuleBehaviour
{
    PlayerStats playerStats;
    public override void Apply(int _newStage)
    {
        playerStats = PlayerStats.instance;
        switch (_newStage)
        {
            case 1:
                playerStats.polarizeTime = 3;
                playerStats.polarizeCooldown = 10;
                break;
            case 2:
                playerStats.polarizeTime = 5;
                playerStats.polarizeCooldown = 10;
                break;
            case 3:
                playerStats.polarizeTime = 5;
                playerStats.polarizeCooldown = 7;
                break;
            case 4:
                playerStats.polarizeTime = 7;
                playerStats.polarizeCooldown = 7;
                break;
            case 5:
                playerStats.polarizeTime = 7;
                playerStats.polarizeCooldown = 5;
                break;
            case 6:
                playerStats.polarizeTime = 10;
                playerStats.polarizeCooldown = 5;
                break;
        }
    }
}
