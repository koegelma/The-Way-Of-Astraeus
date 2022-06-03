using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StageCleared : MonoBehaviour
{
    [SerializeField] private GameObject moduleUpgradeUI;
    public GameObject inGameCanvass;
    public SoundManager soundManager;
    public Text enemiesAmount;
    public int enemiesKilled;
    public float enemiesDisp;

    private void OnEnable()
    {
        soundManager.PlayStageCleared();
        inGameCanvass.SetActive(false);
        StartCoroutine(GetEnemiesKilled());
    }

    private IEnumerator GetEnemiesKilled()
    {
        yield return new WaitUntil(() => enemiesKilled > 0);
        //enemiesAmount.text = enemiesKilled.ToString();

        while (enemiesDisp < enemiesKilled)
        {
            enemiesDisp += Time.deltaTime * enemiesKilled / 0.5f;
            if (enemiesDisp >= enemiesKilled) enemiesDisp = enemiesKilled;

            enemiesAmount.text = Mathf.RoundToInt(enemiesDisp).ToString("#,#");
            yield return null;
        }
    }

    public void ContinueToModules()
    {
        moduleUpgradeUI.SetActive(true);
        gameObject.SetActive(false);
    }
}
