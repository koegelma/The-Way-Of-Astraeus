using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Text totalDamage;

    private void OnEnable()
    {
        totalDamage.text = Mathf.RoundToInt(GameManager.instance.GetTotalDamage()).ToString("#,#");
        //Time.timeScale = 0;
    }

    public void Continue()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
