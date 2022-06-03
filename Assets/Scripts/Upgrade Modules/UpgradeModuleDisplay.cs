using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeModuleDisplay : MonoBehaviour
{
    public SoundManager soundManager;
    private PlayerStats playerStats;
    private List<UpgradeModule> validUpgradeModules;
    private UpgradeModule[] upgradeModulesDisplayed;
    public UpgradeModule[] upgradeModules;
    UpgradeModule upgradeModulePicked = null;
    public Text[] nameTexts;
    public Text[] descriptionTexts;
    public Text[] stageTexts;
    public Toggle[] toggles;
    public Canvas[] upgradeCanvass;

    private void Start()
    {
        playerStats = PlayerStats.instance;

        validUpgradeModules = new List<UpgradeModule>();
        for (int i = 0; i < upgradeModules.Length; i++)
        {
            if (upgradeModules[i].name == "ASSAULT" && !playerStats.isBallistic) continue;
            if (upgradeModules[i].name == "POLARIZE" && playerStats.isBallistic) continue;
            if (playerStats.GetCurrentStage(upgradeModules[i].name) < 6) validUpgradeModules.Add(upgradeModules[i]);
        }

        upgradeModulesDisplayed = new UpgradeModule[3];

        upgradeModulesDisplayed[0] = GetRandomUpgradeModule();
        upgradeModulesDisplayed[1] = GetRandomUpgradeModule();
        upgradeModulesDisplayed[2] = GetRandomUpgradeModule();

        int nullCounter = 0;
        for (int i = 0; i < upgradeModulesDisplayed.Length; i++)
        {
            if (upgradeModulesDisplayed[i] == null)
            {
                nullCounter++;
                toggles[i].interactable = false;
                upgradeCanvass[i].enabled = false;
                continue;
            }
            nameTexts[i].text = upgradeModulesDisplayed[i].name;
            descriptionTexts[i].text = upgradeModulesDisplayed[i].GetDescription();
            stageTexts[i].text = (upgradeModulesDisplayed[i].stage + 1).ToString();
            if (upgradeModulesDisplayed[i].stage + 1 == 6) stageTexts[i].text += " - <color=green>MAX</color>";
        }
        if (nullCounter == upgradeModulesDisplayed.Length) SceneChanger.instance.ChangeScene("0_Menu", false); // ----- TESTING -----
        upgradeModulePicked = upgradeModulesDisplayed[0];
    }

    private UpgradeModule GetRandomUpgradeModule()
    {
        if (validUpgradeModules.Count < 1) return null;
        int random = Random.Range(0, validUpgradeModules.Count);
        UpgradeModule randomUpgrade = validUpgradeModules[random];
        validUpgradeModules.Remove(validUpgradeModules[random]);
        return randomUpgrade;
    }

    public void ToggleFirstCard(bool _value)
    {
        if (_value) upgradeModulePicked = upgradeModulesDisplayed[0];
    }

    public void ToggleSecondCard(bool _value)
    {
        if (_value) upgradeModulePicked = upgradeModulesDisplayed[1];
    }

    public void ToggleThirdCard(bool _value)
    {
        if (_value) upgradeModulePicked = upgradeModulesDisplayed[2];
    }

    public void Continue()
    {
        if (upgradeModulePicked == null)
        {
            Debug.Log("No upgrade picked");
            return;
        }
        upgradeModulePicked.Apply();
        // change to shop / next scene
        StartCoroutine(GameManager.instance.SaveAndReloadScene());
    }
}