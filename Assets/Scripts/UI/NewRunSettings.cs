using UnityEngine;


public class NewRunSettings : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerUpgrades.instance.isBallistic = true;
    }

    public void ToggleBallistic(bool _value)
    {
        if (_value) PlayerUpgrades.instance.isBallistic = true;
    }

    public void ToggleEnergy(bool _value)
    {
        if (_value) PlayerUpgrades.instance.isBallistic = false;
    }
}
