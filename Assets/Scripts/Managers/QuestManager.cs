using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class QuestManager : MonoBehaviour
{
    private float liveTime = 0f;
    private float liveTimeLevel1Cut = 3f;
    private float liveTimeLevel2Cut = 10f;
    private bool liveTime1Checked = false;
    private bool liveTimeLevel2Checked = false;

    private float attackPercent = 0f;
    private float attackPercentLevel1Cut;
    private float attackPercentLevel2Cut;
    private bool attackPercent1Checked = false;
    private bool attackPercent2Checked = false;

    private int[] patternSeeCount = new int[5]{0, 0, 0, 0, 0};
    private int patternSeeCountLevel1Cut;
    private int patternSeeCountLevel2Cut;
    private bool[] patternSeeCount1Checked = new bool[5]{false, false, false, false, false};
    private bool[] patternSeeCount2Checked = new bool[5]{false, false, false, false, false};

    private int justAvoidCount = 0; 
    private int justAvoidCountLevel1Cut;
    private int justAvoidCountLevel2Cut;
    private bool justAvoidCount1Checked = false;
    private bool justAvoidCount2Checked = false;
    private BookManager bookManager;
    [SerializeField] private Boss boss;
    private float questDurationTime = 15f;
    [SerializeField] private List<GameObject> questPanels;
    [HideInInspector] public string NowQuestBookName;
    [HideInInspector] public int NowQuestLevel;
    [HideInInspector] public bool StopTimer = false; // it should be changed in PlayerBehaviour when Player write the history book
    [HideInInspector] public bool UnlockBook = false;
    private string bossName;
    
    private void Start()
    {
        bookManager = BookManager.Instance;
        if(boss is Thor)
        {
            bossName = "Thor";
        }
        /*
        else if(boss is Surtr)
        {
            bossName = "Surtr";
        }
        */
    }

    private void Update()
    {
        liveTime += Time.deltaTime;
        if (liveTime > liveTimeLevel2Cut && !liveTimeLevel2Checked)
        {
            liveTimeLevel2Checked = true;
            ReadytoWriteBook("Tenacity", 2);
        }
        else if (liveTime > liveTimeLevel1Cut && !liveTime1Checked)
        {
            liveTime1Checked = true;
            ReadytoWriteBook("Tenacity", 1);
        }
    }

    public void CheckAttackPercent()
    {
        attackPercent = 1f - boss.NowHP/boss.MaxHP;
        if (attackPercent > attackPercentLevel2Cut && !attackPercent2Checked)
        {
            attackPercent2Checked = true;
            ReadytoWriteBook("Challenge", 2);
        }
        else if (attackPercent > attackPercentLevel1Cut && !attackPercent1Checked)
        {
            attackPercent1Checked = true;
            ReadytoWriteBook("Challenge", 1);
        }
    }

    public void UpPatternSeeCount(int patternNum)
    {
        patternSeeCount[patternNum] += 1;
        if (patternSeeCount[patternNum] >= patternSeeCountLevel2Cut && !patternSeeCount2Checked[patternNum])
        {
            patternSeeCount2Checked[patternNum] = true;
            ReadytoWriteBook(bossName + (patternNum + 1).ToString(), 2);
        }
        else if (patternSeeCount[patternNum] >= patternSeeCountLevel1Cut && !patternSeeCount1Checked[patternNum])
        {
            patternSeeCount1Checked[patternNum] = true;
            ReadytoWriteBook(bossName + (patternNum + 1).ToString(), 1);
        }
    }

    public void UpJustAvoidCount()
    {
        justAvoidCount += 1;
        if (justAvoidCount >= justAvoidCountLevel2Cut && !justAvoidCount2Checked)
        {
            justAvoidCount2Checked = true;
            ReadytoWriteBook("Alertness", 2);
        }
        else if (justAvoidCount >= justAvoidCountLevel1Cut && !justAvoidCount1Checked)
        {
            justAvoidCount1Checked = true;
            ReadytoWriteBook("Alertness", 1);
        }
    }

    public void ReadytoWriteBook(string bookName, int level) 
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
                SetQuestPanel(i, bookName, level);
                break;
            }
        }
    }

    public void SetQuestPanel(int questPanelNum, string bookName, int level)
    {
        QuestPanel questPanel = questPanels[questPanelNum].GetComponent<QuestPanel>();
        TMP_Text questText = questPanels[questPanelNum].GetComponentInChildren<TMP_Text>();
        Slider durationSlider = questPanels[questPanelNum].GetComponentInChildren<Slider>();
        durationSlider.value = 1f;
        questPanel.BookName = bookName;
        questPanel.Level = level;

        questText.text = "It's able to write the history book.\nPress[F] to write";
        StartCoroutine(QuestPanelStart(questPanel, durationSlider));
    }

    public IEnumerator QuestPanelStart(QuestPanel questPanel, Slider slider)
    {
        while (slider.value > 0f)
        {
            if (!StopTimer)
            {
                slider.value -= Time.deltaTime/questDurationTime;
            }
            if (UnlockBook)
            {
                bookManager.SetBookUnlocked(questPanel.BookName, questPanel.Level);
                StopTimer = false;
                break;
            }
            yield return null;
        }
        UnlockBook = false;
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
                    questPanels[i].GetComponent<QuestPanel>().BookName = questPanels[i+1].GetComponent<QuestPanel>().BookName;
                    questPanels[i].GetComponent<QuestPanel>().Level = questPanels[i+1].GetComponent<QuestPanel>().Level;
                    questPanels[i].GetComponentInChildren<Slider>().value = questPanels[i+1].GetComponentInChildren<Slider>().value;
                    StartCoroutine(QuestPanelStart(questPanels[i].GetComponent<QuestPanel>(), questPanels[i].GetComponentInChildren<Slider>()));
                }
                else
                {
                    questPanels[i].SetActive(false);
                    break;
                }
            }
        }
        
    }
}
