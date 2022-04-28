using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Text totalDamage;

    private void OnEnable()
    {
        totalDamage.text = Mathf.RoundToInt(GameManager.instance.TotalDamage).ToString("#,#");
    }

    public void Continue()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
