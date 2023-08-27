using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TitleSceneMover : MonoBehaviour
{
    public GameObject Story;
    public GameObject Tutorial1;
    public GameObject Tutorial2;

    static public bool StoryEnd = false;

    void Update()
    {
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            
            if (Story.activeSelf == false)
            {

                StartCoroutine(MoveToStory());
            }

            else if (Tutorial1.activeSelf == false)
            {
                if (StoryEnd == true)
                {
                    StartCoroutine(MoveToTutorial1());
                }
            }

            else
            {
                if (Tutorial2.activeSelf == false)
                {
                    StartCoroutine(MoveToTutorial2());
                }
                else
                {
                    SceneLoader.Instance.LoadStoreScene();
                }
            }
        }
    }

    IEnumerator MoveToStory()
    {
        SceneLoader.Instance.BlackScreen.SetActive(true);
        var tweening = SceneLoader.Instance.canvasGroup.DOFade(1f, 0.5f);
        yield return tweening.WaitForCompletion();
        Story.SetActive(true);
        AudioManager.Instance.PlaySfx(8);
        tweening = SceneLoader.Instance.canvasGroup.DOFade(0f, 0.5f);
        yield return tweening.WaitForCompletion();
        SceneLoader.Instance.BlackScreen.SetActive(false);
    }

    IEnumerator MoveToTutorial1()
    {
        SceneLoader.Instance.BlackScreen.SetActive(true);
        var tweening = SceneLoader.Instance.canvasGroup.DOFade(1f, 0.5f);
        yield return tweening.WaitForCompletion();
        Tutorial1.SetActive(true);
        AudioManager.Instance.PlaySfx(8);
        tweening = SceneLoader.Instance.canvasGroup.DOFade(0f, 0.5f);
        yield return tweening.WaitForCompletion();
        SceneLoader.Instance.BlackScreen.SetActive(false);
    }

    IEnumerator MoveToTutorial2()
    {
        SceneLoader.Instance.BlackScreen.SetActive(true);
        var tweening = SceneLoader.Instance.canvasGroup.DOFade(1f, 0.5f);
        yield return tweening.WaitForCompletion();
        Tutorial2.SetActive(true);
        AudioManager.Instance.PlaySfx(8);
        tweening = SceneLoader.Instance.canvasGroup.DOFade(0f, 0.5f);
        yield return tweening.WaitForCompletion();
        SceneLoader.Instance.BlackScreen.SetActive(false);
    }
}
