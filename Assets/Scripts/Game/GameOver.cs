using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Text totalDamage;
    private float damageDisp;

    private void OnEnable()
    {
        //totalDamage.text = Mathf.RoundToInt(GameManager.instance.TotalDamage).ToString("#,#");
        damageDisp = 0;
    }

    private void Update()
    {
        damageDisp += Time.deltaTime * GameManager.instance.TotalDamage;
        if (damageDisp >= GameManager.instance.TotalDamage) damageDisp = GameManager.instance.TotalDamage;
        totalDamage.text = Mathf.RoundToInt(damageDisp).ToString("#,#");
    }

    public void Continue()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
