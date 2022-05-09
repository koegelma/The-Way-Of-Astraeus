using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Text totalDamage;
    private float damageDisp;

    private void OnEnable()
    {
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
        //saveLoadSystem.Save();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("0_Menu"); // go to perma talent tree, or menu and invest talents there?
    }
}
