using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeModuleDisplay : MonoBehaviour
{
    public UpgradeModule upgradeModule;
    public Text nameText;
    public Text descriptionText;
    public Text stageText;

    private void Start()
    {
        nameText.text = upgradeModule.name;
        descriptionText.text = upgradeModule.GetDescription();
        stageText.text = (upgradeModule.stage + 1).ToString();
    }
}
