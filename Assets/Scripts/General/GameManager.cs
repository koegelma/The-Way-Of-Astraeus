using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public SoundManager soundManager;
    private PlayerStats playerStats;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private SaveLoadSystem saveLoadSystem;
    public bool gameEnded;
    public GameObject playerShip;

    private void Awake()
    {
        instance = this;
        saveLoadSystem.Load();
    }
    private void Start()
    {
        Time.timeScale = 1;
        gameEnded = false;
        playerStats = PlayerStats.instance;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (playerHealth.health <= 0) StartCoroutine(EndGame());
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(1);
        gameEnded = true;
        gameOverUI.SetActive(true);
        Cursor.visible = true;
    }

    public IEnumerator SaveAndReloadScene()
    {
        saveLoadSystem.Save();
        yield return new WaitUntil(() => saveLoadSystem.hasSaved);
        SceneChanger.instance.ChangeScene(SceneManager.GetActiveScene().name, false);
    }
}
