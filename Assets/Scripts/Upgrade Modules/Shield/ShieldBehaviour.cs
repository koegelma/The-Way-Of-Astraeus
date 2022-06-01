using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade Module Behaviour/Shield")]
public class ShieldBehaviour : UpgradeModuleBehaviour
{
    PlayerStats playerStats;
    public override void Apply(int _newStage)
    {
        playerStats = PlayerStats.instance;
        switch (_newStage)
        {
            case 1:
                playerStats.shieldAmount = 25;
                playerStats.shieldCooldown = 7;
                break;
            case 2:
                playerStats.shieldAmount = 25;
                playerStats.shieldCooldown = 5;
                break;
            case 3:
                playerStats.shieldAmount = 50;
                playerStats.shieldCooldown = 5;
                break;
            case 4:
                playerStats.shieldAmount = 50;
                playerStats.shieldCooldown = 3;
                break;
            case 5:
                playerStats.shieldAmount = 75;
                playerStats.shieldCooldown = 3;
                break;
            case 6:
                playerStats.shieldAmount = 100;
                playerStats.shieldCooldown = 3;
                break;
        }
    }
}
