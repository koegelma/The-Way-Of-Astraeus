using UnityEngine;
[CreateAssetMenu(menuName = "Upgrade Module Behaviour/Critical Damage")]
public class CriticalDamageBehaviour : UpgradeModuleBehaviour
{
    PlayerStats playerStats;
    public override void Apply(int _newStage)
    {
        playerStats = PlayerStats.instance;
        switch (_newStage)
        {
            case 1:
                playerStats.critChance = 0.1f;
                playerStats.critDMG = 0.5f;
                break;
            case 2:
                playerStats.critChance = 0.1f;
                playerStats.critDMG = 0.75f;
                break;
            case 3:
                playerStats.critChance = 0.25f;
                playerStats.critDMG = 0.75f;
                break;
            case 4:
                playerStats.critChance = 0.25f;
                playerStats.critDMG = 1;
                break;
            case 5:
                playerStats.critChance = 0.5f;
                playerStats.critDMG = 1;
                break;
            case 6:
                playerStats.critChance = 1;
                playerStats.critDMG = 2;
                playerStats.secHasCrit = true;
                break;
        }
    }
}
