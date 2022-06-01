using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject ui;
    [SerializeField] private GameObject newRunUI;
    //[SerializeField] private GameObject talentsUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private SaveLoadSystem saveLoadSystem;

    private void Start()
    {
        Time.timeScale = 1;
    }

    public void ToggleNewRunUI()
    {
        newRunUI.SetActive(!newRunUI.activeSelf);
        if (newRunUI.activeSelf) ui.SetActive(false);
        else ui.SetActive(true);
    }

    public void NewRun()
    {
        SceneChanger.instance.ChangeScene("1_Stage", true);
    }


    public void ToggleTalents()
    {
        Debug.Log("Talents screen here.");
    }

    public void ToggleOptions()
    {
        optionsUI.SetActive(!optionsUI.activeSelf);
        if (optionsUI.activeSelf) ui.SetActive(false);
        else ui.SetActive(true);
    }

    public void ExitApplication()
    {
        saveLoadSystem.Save();
        StartCoroutine(Exit());
    }

    private IEnumerator Exit()
    {
        yield return new WaitUntil(() => saveLoadSystem.hasSaved);
        Application.Quit();
    }
}
