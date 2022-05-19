using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject ui;
    [SerializeField] private GameObject newRunUI;
    //[SerializeField] private GameObject talentsUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private MenuShip menuShip;
    [SerializeField] private SaveLoadSystem saveLoadSystem;
    private Animator animator;

    private void Start()
    {
        Time.timeScale = 1;
        animator = GetComponent<Animator>();
    }

    public void ToggleNewRunUI()
    {
        newRunUI.SetActive(!newRunUI.activeSelf);
        if (newRunUI.activeSelf) ui.SetActive(false);
        else ui.SetActive(true);
    }

    public void NewRun()
    {
        if (!newRunUI.GetComponent<NewRunSettings>().weaponSysChoosen)
        {
            newRunUI.transform.GetChild(0).gameObject.SetActive(true);
            return;
        }
        // check if weapon system picked
        saveLoadSystem.Save();
        StartCoroutine(StartNewRun());
    }

    private IEnumerator StartNewRun()
    {
        animator.SetTrigger("TrFadeOut");
        menuShip.newGameStarting = true;
        yield return new WaitUntil(() => menuShip.shipIsInPosition && saveLoadSystem.hasSaved);
        SceneManager.LoadScene("1_Stage");
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
