using UnityEngine;
using UnityEngine.UI;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField] private Text cAmount;
    [SerializeField] private Text sAmount;
    private PlayerStats playerStats;

    private void Start()
    {
        playerStats = PlayerStats.instance;

        cAmount.text = GetFormattedString(playerStats.cAmount);
        sAmount.text = GetFormattedString(playerStats.sAmount);
    }

    public void UpdateCoinText()
    {
        cAmount.text = GetFormattedString(playerStats.cAmount);
    }

    public void UpdateSkillCurrText()
    {
        sAmount.text = GetFormattedString(playerStats.sAmount);
    }

    private string GetFormattedString(int _currentAmount)
    {
        if (_currentAmount == 0) return _currentAmount.ToString();
        else return _currentAmount.ToString("#,#");
    }
}
