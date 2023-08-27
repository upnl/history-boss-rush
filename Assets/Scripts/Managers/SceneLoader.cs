using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneLoader : MonoBehaviour
{
    private static SceneLoader _Instance;
    public GameObject BlackScreen;
    public CanvasGroup canvasGroup;

    void Awake()
    {
        if (null == _Instance)
        {
            _Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static SceneLoader Instance
    {
        get
        {
            if (null == _Instance)
            {
                return null;
            }
            return _Instance;
        }
    }

    private void Start()
    {
        canvasGroup = BlackScreen.GetComponent<CanvasGroup>();
    }

    public void LoadTitleScene()
    {
        StartCoroutine(LoadSceneAsync("Title"));
    }

    public void LoadStoreScene()
    {
        StartCoroutine(LoadSceneAsync("StoreScene"));
    }

    public void LoadFieldScene()
    {
        StartCoroutine(LoadSceneAsync("FieldScene"));
    }

    public void LoadThorScene()
    {
        StartCoroutine(LoadSceneAsync("Boss_Thor"));
    }

    public void LoadSurtrScene()
    {
        StartCoroutine(LoadSceneAsync("Boss_Surtr 2"));
    }

    IEnumerator LoadSceneAsync(string levelToLoad)
    {
        BlackScreen.SetActive(true);
        var tweening = canvasGroup.DOFade(1f, 0.5f);

        yield return tweening.WaitForCompletion();
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

        while (!loadOperation.isDone)
        {
            yield return null;
        }

        tweening = canvasGroup.DOFade(0f, 0.5f);

        yield return tweening.WaitForCompletion();
        BlackScreen.SetActive(false);
    }

    public void EnableTransition()
    {
        StartCoroutine(ScreenTransition());
    }

    IEnumerator ScreenTransition()
    {
        BlackScreen.SetActive(true);
        var tweening = canvasGroup.DOFade(1f, 0.5f);
        yield return tweening.WaitForCompletion();
        tweening = canvasGroup.DOFade(0f, 0.5f);
        yield return tweening.WaitForCompletion();
        BlackScreen.SetActive(false);
    }
}