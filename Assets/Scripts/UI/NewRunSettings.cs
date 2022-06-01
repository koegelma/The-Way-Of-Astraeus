using UnityEngine;

public class NewRunSettings : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerStats.instance.isBallistic = true;
    }

    public void ToggleBallistic(bool _value)
    {
        if (_value) PlayerStats.instance.isBallistic = true;
    }

    public void ToggleEnergy(bool _value)
    {
        if (_value) PlayerStats.instance.isBallistic = false;
    }
}
