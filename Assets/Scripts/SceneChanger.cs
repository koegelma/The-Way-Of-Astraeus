using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour, ISaveable
{
    public static SceneChanger instance;
    public Image fadeImg;
    public AnimationCurve curve;
    private float speed = 4f;
    public SaveLoadSystem saveLoadSystem;
    private bool initializeNewRun = false;
    private bool hasLoaded = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void ChangeScene(string _scene, bool _initializeNewRun)
    {
        initializeNewRun = _initializeNewRun;
        StartCoroutine(FadeOut(_scene));
    }

    private IEnumerator FadeIn()
    {
        float t = 1f;

        while (t > 0f)
        {
            t -= Time.deltaTime * speed;
            float a = curve.Evaluate(t);
            fadeImg.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }

        yield return new WaitUntil(() => hasLoaded);
        if (initializeNewRun) StartCoroutine(PlayerStats.instance.InitializeNewRun());
    }

    private IEnumerator FadeOut(string _scene)
    {
        Time.timeScale = 1;
        
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            float a = curve.Evaluate(t);
            fadeImg.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }
        saveLoadSystem.Save();
        yield return new WaitUntil(() => saveLoadSystem.hasSaved);
        SceneManager.LoadScene(_scene);
    }

    public object SaveState()
    {
        return new SaveData()
        {
            initializeNewRun = this.initializeNewRun
        };
    }

    public void LoadState(object state)
    {
        var saveData = (SaveData)state;
        initializeNewRun = saveData.initializeNewRun;

        hasLoaded = true;
    }

    [Serializable]
    private struct SaveData
    {
        public bool initializeNewRun;
    }
}
