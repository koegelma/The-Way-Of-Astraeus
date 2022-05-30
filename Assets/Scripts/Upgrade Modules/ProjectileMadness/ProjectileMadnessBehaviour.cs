using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade Module Behaviour/Projectile Madness")]
public class ProjectileMadnessBehaviour : UpgradeModuleBehaviour
{
    private PlayerStats playerStats;
    public override void Apply(int _newStage)
    {
        playerStats = PlayerStats.instance;

        switch (_newStage)
        {
            case 1:
                playerStats.projectileAmount = 2;
                break;
            case 2:
                playerStats.projectileAmount = 3;
                break;
            case 3:
                playerStats.projectileAmount = 4;
                break;
            case 4:
                playerStats.projectileAmount = 5;
                break;
            case 5:
                playerStats.projectileAmount = 6;
                break;
            case 6:
                playerStats.secHasMultipleProjectiles = true;
                break;
        }
    }
}
