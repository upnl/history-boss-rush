using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class TitleTextAnimator : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText1;
    [SerializeField] private TMP_Text titleText2;
    [SerializeField] private TMP_Text titleText3;
    [SerializeField] private TitleSceneMover titleSceneMover;
    private void Start()
    {
        StartCoroutine(TitleLine());
    }

    private IEnumerator TitleLine()
    {
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(TextColor(titleText1));
        yield return new WaitForSeconds(0.3f);
        yield return StartCoroutine(TextColor(titleText2));
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(TextColor(titleText3));

        titleSceneMover.gameObject.SetActive(true);
        StartCoroutine(TextBlinking(titleText3));
    }

    private IEnumerator TextColor(TMP_Text text)
    {
        float elapsedTime = 0f;
        while(elapsedTime < 0.7f)
        {
            elapsedTime += Time.deltaTime;
            text.color = new Color(text.color.r, text.color.g, text.color.b, elapsedTime/0.6f);
            yield return null;
        }
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);
    }

    private IEnumerator TextBlinking(TMP_Text text)
    {
        float elapsedTime = 0f;
        while(true)
        {
            elapsedTime += Time.deltaTime;
            text.color = new Color(text.color.r, text.color.g, text.color.b, 2f*(Mathf.Sin(elapsedTime)+1f)/2f);
            yield return null;
        }
    }
}
