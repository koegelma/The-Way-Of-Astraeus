using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public SoundManager soundManager;
    private PlayerStats playerStats;
    [SerializeField] private Text totalDamage;
    private float damageDisp;
    private bool totalDamageReached;
    public GameObject inGameCanvass;

    private void Start()
    {
        playerStats = PlayerStats.instance;
    }

    private void OnEnable()
    {
        damageDisp = 0;
        soundManager.PlayGameOver();
        inGameCanvass.SetActive(false);
    }

    private void Update()
    {
        if (totalDamageReached) return;
        damageDisp += Time.deltaTime * playerStats.totalDamage;
        if (damageDisp >= playerStats.totalDamage)
        {
            damageDisp = playerStats.totalDamage;
            totalDamageReached = true;
        }
        totalDamage.text = Mathf.RoundToInt(damageDisp).ToString("#,#");
    }

    public void Continue()
    {
        SceneChanger.instance.ChangeScene("0_Menu", false);
    }
}
