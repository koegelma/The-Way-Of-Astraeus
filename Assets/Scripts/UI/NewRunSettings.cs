using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewRunSettings : MonoBehaviour
{
    public bool weaponSysChoosen;

    private void OnEnable()
    {
        weaponSysChoosen = false;
    }

    public void ChooseBallistic()
    {
        weaponSysChoosen = true;
        if (transform.GetChild(0).gameObject.activeSelf) transform.GetChild(0).gameObject.SetActive(false);
        PlayerUpgrades.instance.isBallistic = true;
    }

    public void ChooseEnergy()
    {
        weaponSysChoosen = true;
        if (transform.GetChild(0).gameObject.activeSelf) transform.GetChild(0).gameObject.SetActive(false);
        PlayerUpgrades.instance.isBallistic = false;
    }

}
