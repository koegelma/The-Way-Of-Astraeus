using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade Module Behaviour/Leech")]
public class LeechBehaviour : UpgradeModuleBehaviour
{
    PlayerStats playerStats;
    public override void Apply(int _newStage)
    {
        playerStats = PlayerStats.instance;
        switch (_newStage)
        {
            case 1:
                playerStats.leechChance = 0.1f;
                playerStats.leechAmount = 0.01f;
                break;
            case 2:
                playerStats.leechChance = 0.2f;
                playerStats.leechAmount = 0.01f;
                break;
            case 3:
                playerStats.leechChance = 0.2f;
                playerStats.leechAmount = 0.02f;
                break;
            case 4:
                playerStats.leechChance = 0.3f;
                playerStats.leechAmount = 0.02f;
                break;
            case 5:
                playerStats.leechChance = 0.3f;
                playerStats.leechAmount = 0.03f;
                break;
            case 6:
                playerStats.leechChance = 0.5f;
                playerStats.leechAmount = 0.05f;
                break;
        }
    }
}
