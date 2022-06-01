using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade Module Behaviour/Armor")]
public class ArmorBehaviour : UpgradeModuleBehaviour
{
    PlayerStats playerStats;
    public override void Apply(int _newStage)
    {
        playerStats = PlayerStats.instance;
        switch (_newStage)
        {
            case 1:
                playerStats.armorAmount = 1;
                break;
            case 2:
                playerStats.armorAmount = 2;
                break;
            case 3:
                playerStats.armorAmount = 5;
                break;
            case 4:
                playerStats.armorAmount = 10;
                break;
            case 5:
                playerStats.armorAmount = 15;
                break;
            case 6:
                playerStats.armorAmount = 20;
                playerStats.armorReflAmount = 0.5f;
                break;
        }
    }
}
