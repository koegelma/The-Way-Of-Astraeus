using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeModuleDisplay : MonoBehaviour
{
    private PlayerStats playerStats;
    private List<UpgradeModule> validUpgradeModules; // save on scene change. only remove upgrade module if last stage is acquired
    private UpgradeModule[] upgradeModulesDisplayed;
    private bool firstPicked;
    private bool secondPicked;
    private bool thirdPicked;
    public UpgradeModule[] upgradeModules;
    public Text[] nameTexts;
    public Text[] descriptionTexts;
    public Text[] stageTexts;

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
                continue;
            }
            nameTexts[i].text = upgradeModulesDisplayed[i].name;
            descriptionTexts[i].text = upgradeModulesDisplayed[i].GetDescription();
            stageTexts[i].text = (upgradeModulesDisplayed[i].stage + 1).ToString();
        }
        if (nullCounter >= 3) SceneChanger.instance.ChangeScene("0_Menu", false);
    }

    private void OnEnable()
    {
        firstPicked = true;
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
        if (_value)
        {
            firstPicked = true;
            secondPicked = false;
            thirdPicked = false;
        }
    }

    public void ToggleSecondCard(bool _value)
    {
        if (_value)
        {
            firstPicked = false;
            secondPicked = true;
            thirdPicked = false;
        }
    }

    public void ToggleThirdCard(bool _value)
    {
        if (_value)
        {
            firstPicked = false;
            secondPicked = false;
            thirdPicked = true;
        }
    }

    public void Continue()
    {
        UpgradeModule upgradeModulePicked = null;

        if (firstPicked) upgradeModulePicked = upgradeModulesDisplayed[0];//newStage = upgradeModulesDisplayed[0].Apply();
        if (secondPicked) upgradeModulePicked = upgradeModulesDisplayed[1];//newStage = upgradeModulesDisplayed[1].Apply();
        if (thirdPicked) upgradeModulePicked = upgradeModulesDisplayed[2];//newStage = upgradeModulesDisplayed[2].Apply();

        if (upgradeModulePicked == null)
        {
            Debug.Log("Pick other upgrade");
            return;
        }

        int newStage = upgradeModulePicked.Apply();

        if (newStage == 6)
        {
            foreach (UpgradeModule validUpgradeModule in validUpgradeModules)
            {
                if (validUpgradeModule.name == upgradeModulePicked.name)
                {
                    validUpgradeModules.Remove(validUpgradeModule);
                    Debug.Log(validUpgradeModule.name + " max stage reached.");
                }
            }
        }


        // change to shop / next scene

        StartCoroutine(GameManager.instance.SaveAndReloadScene());
    }
}