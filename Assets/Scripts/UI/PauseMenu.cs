using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private GameObject ui;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject exitRunUI;

    private void Start()
    {
        gameManager = GameManager.instance;
    }

    private void Update()
    {
        if (gameManager.gameEnded) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
            if (optionsUI.activeSelf) ToggleOptions();
            if (exitRunUI.activeSelf) ToggleExitRun();
        }
    }

    public void TogglePauseMenu()
    {
        ui.SetActive(!ui.activeSelf);
        if (ui.activeSelf)
        {
            Time.timeScale = 0;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1;
            Cursor.visible = false;
        }
    }

    public void ToggleOptions()
    {
        optionsUI.SetActive(!optionsUI.activeSelf);
        if (optionsUI.activeSelf) ui.SetActive(false);
        else ui.SetActive(true);
    }

    public void ToggleExitRun()
    {
        exitRunUI.SetActive(!exitRunUI.activeSelf);
    }

    public void ConfirmExitRun()
    {
        SceneChanger.instance.ChangeScene("0_Menu", false);
    }
}
