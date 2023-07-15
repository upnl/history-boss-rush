using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class QuestManager : MonoBehaviour
{
    private float liveTime = 0f;
    private float liveTimeLevel1Cut;
    private float liveTimeLevel2Cut;

    private float attackPercent = 0f;
    private float attackPercentLevel1Cut;
    private float attackPercentLevel2Cut;

    private int[] patternSeeCount = new int[5]{0, 0, 0, 0, 0};
    private int patternSeeCountLevel1Cut;
    private int patternSeeCountLevel2Cut;

    private int justAvoidCount = 0; 
    private int justAvoidCountLevel1Cut;
    private int justAvoidCountLevel2Cut;
    private BookManager bookManager;
    [SerializeField] private Boss boss;
    private float questDurationTime;
    [SerializeField] private List<GameObject> questPanels;
    [HideInInspector] public string NowQuestBookName;
    [HideInInspector] public int NowQuestLevel;
    [HideInInspector] public bool StopTimer = false; // it should be changed in PlayerBehaviour when Player write the history book
    private string bossName;

    private void Start()
    {
        bookManager = BookManager.Instance;
        /*
        if(boss is Thor)
        {
            bossName = "Thor";
        }
        else if(boss is Surtr)
        {
            bossName = "Surtr";
        }
        */
        bossName = "Thor"; // temporary code
    }

    private void Update()
    {
        liveTime += Time.deltaTime;
        if (liveTime > liveTimeLevel2Cut)
        {
            ReadytoWriteBook("tenacity", 2);
        }
        else if (liveTime > liveTimeLevel1Cut)
        {
            ReadytoWriteBook("tenacity", 1);
        }
    }

    public void CheckAttackPercent()
    {
        attackPercent = 1f - boss.NowHP/boss.MaxHP;
        if (attackPercent > attackPercentLevel2Cut)
        {
            ReadytoWriteBook("challenge", 2);
        }
        else if (attackPercent > attackPercentLevel1Cut)
        {
            ReadytoWriteBook("challenge", 1);
        }
    }

    public void UpPatternSeeCount(int patternNum)
    {
        patternSeeCount[patternNum] += 1;
        if (patternSeeCount[patternNum] >= patternSeeCountLevel2Cut)
        {
            ReadytoWriteBook(bossName + (patternNum + 1).ToString(), 2);
        }
        else if (patternSeeCount[patternNum] >= patternSeeCountLevel1Cut)
        {
            ReadytoWriteBook(bossName + (patternNum + 1).ToString(), 1);
        }
    }

    public void UpJustAvoidCount()
    {
        justAvoidCount += 1;
        if (justAvoidCount >= justAvoidCountLevel2Cut)
        {
            ReadytoWriteBook("alertness", 2);
        }
        else if (justAvoidCount >= justAvoidCountLevel1Cut)
        {
            ReadytoWriteBook("alertness", 1);
        }
    }

    public void ReadytoWriteBook(string bookName, int level, int patternNum = 0) 
    {
        for (int i=0; i<questPanels.Count; i++)
        {
            if (questPanels[i].activeSelf)
            {
                continue;
            }
            else
            {
                if(i==0)
                {
                    NowQuestBookName = bookName;
                    NowQuestLevel = level;
                }
                questPanels[i].SetActive(true);
                SetQuestPanel(i, bookName, level, patternNum);
            }
        }
    }

    public void SetQuestPanel(int questPanelNum, string bookName, int level, int patternNum = 0)
    {
        TMP_Text questText = questPanels[questPanelNum].GetComponentInChildren<TMP_Text>();
        Slider durationSlider = questPanels[questPanelNum].GetComponentInChildren<Slider>();

        questText.text = "It's able to write the history book.\nPRess[F] to write";
        StartCoroutine(QuestPanelStart(durationSlider, bookName, level, patternNum));
    }

    public IEnumerator QuestPanelStart(Slider slider, string bookName, int level, int patternNum = 0)
    {
        float elapsedTime = 0f;
        while (elapsedTime < questDurationTime)
        {
            if (!StopTimer)
            {
                elapsedTime += Time.deltaTime;
                slider.value = 1f - elapsedTime/questDurationTime;
            }
            yield return null;
        }
        CloseQuestPanel();
        
    }

    private void CloseQuestPanel()
    {
        for (int i=0; i<questPanels.Count-1; i++)
        {
            if (questPanels[i].activeSelf)
            {
                if (questPanels[i+1].activeSelf)
                {
                    questPanels[i].GetComponentInChildren<Slider>().value = questPanels[i+1].GetComponentInChildren<Slider>().value;
                }
                else
                {
                    questPanels[i].SetActive(false);
                    break;
                }
            }
        }
        StopTimer = false;
    }
}
