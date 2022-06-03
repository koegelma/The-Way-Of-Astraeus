using UnityEngine;
[CreateAssetMenu(menuName = "Upgrade Module Behaviour/Assault")]
public class AssaultBehaviour : UpgradeModuleBehaviour
{
    PlayerStats playerStats;
    public override void Apply(int _newStage)
    {
        playerStats = PlayerStats.instance;
        switch (_newStage)
        {
            case 1:
                playerStats.assaultTime = 3;
                playerStats.assaultCooldown = 10;
                break;
            case 2:
                playerStats.assaultTime = 5;
                playerStats.assaultCooldown = 10;
                break;
            case 3:
                playerStats.assaultTime = 5;
                playerStats.assaultCooldown = 7;
                break;
            case 4:
                playerStats.assaultTime = 7;
                playerStats.assaultCooldown = 7;
                break;
            case 5:
                playerStats.assaultTime = 7;
                playerStats.assaultCooldown = 5;
                break;
            case 6:
                playerStats.assaultTime = 10;
                playerStats.assaultCooldown = 5;
                break;
        }
    }
}
