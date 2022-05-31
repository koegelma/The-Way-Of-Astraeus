using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeModuleDisplay : MonoBehaviour
{
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
        }
        if (nullCounter >= 3) SceneChanger.instance.ChangeScene("0_Menu", false); // ----- TESTING -----
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

        //int newStage = 
        upgradeModulePicked.Apply();

       /*  if (newStage == 6)
        {
            foreach (UpgradeModule validUpgradeModule in validUpgradeModules)
            {
                if (validUpgradeModule.name == upgradeModulePicked.name)
                {
                    validUpgradeModules.Remove(validUpgradeModule);
                    Debug.Log(validUpgradeModule.name + " max stage reached.");
                }
            }
        } */
        // change to shop / next scene

        StartCoroutine(GameManager.instance.SaveAndReloadScene());
    }
}