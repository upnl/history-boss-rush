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
    private bool liveTime1Checked = false;
    private bool liveTimeLevel2Checked = false;

    private float remainedPercent = 0f;
    private int remainedPercentLevel1Cut;
    private int remainedPercentLevel2Cut;
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
    
    [SerializeField] private Boss boss;
    private float questDurationTime = 15f;
    [SerializeField] private List<GameObject> questPanels;
    [HideInInspector] public bool StopTimer = false; 
    [HideInInspector] public bool UnlockBook = false;
    [HideInInspector] public bool HasQuest = false;
    private CSVReader bookDB;
    private string bossName;
    [SerializeField] private TMP_Text bossNameText;
    private Dictionary<string, string> englishBossNameToKorean = new Dictionary<string, string>
    {
        {"Thor", "천둥의 신 토르"},
        {"Surtr", "불의 거인 수르트"}
    };

    private void Start()
    {
        if (boss is Thor)
        {
            bossName = "Thor"; 
        }
        else if (boss is Surtr)
        {
            bossName = "Surtr";
        }
        bookDB = BookManager.Instance.bookDB;
        bossNameText.text = englishBossNameToKorean[bossName];

        liveTimeLevel1Cut = intParseConditionDB("Tenacity",1);
        liveTimeLevel2Cut = intParseConditionDB("Tenacity",2);

        remainedPercentLevel1Cut = intParseConditionDB("Challenge", 1);
        remainedPercentLevel2Cut = intParseConditionDB("Challenge", 2);

        patternSeeCountLevel1Cut = intParseConditionDB("Thor1", 1);
        patternSeeCountLevel2Cut = intParseConditionDB("Thor1", 2);

        // justAvoidCountLevel1Cut = intParseConditionDB("Alertness", 1, 2);
        // justAvoidCountLevel2Cut = intParseConditionDB("Alertness", 2, 2);
    }

    public int intParseConditionDB(string title, int historyLevel, int conditionNum = 1)
    {
        int result = int.Parse(bookDB.GetData().Find(
            e => e[bookDB.GetHeaderIndex("title")].Equals(title) &&
            int.Parse(e[bookDB.GetHeaderIndex("level")]) == historyLevel)[bookDB.GetHeaderIndex("condition"+conditionNum.ToString())]);
        return result;
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
        remainedPercent = (boss.NowHP/boss.MaxHP) * 100;
        if (remainedPercent < remainedPercentLevel2Cut && !attackPercent2Checked)
        {
            attackPercent2Checked = true;
            ReadytoWriteBook("Challenge", 2);
        }
        else if (remainedPercent < remainedPercentLevel1Cut && !attackPercent1Checked)
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

        HasQuest = true;
        questText.text = "역사서를 작성 가능합니다.\n[F]키를 눌러서 작성하세요.";
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
                BookManager.Instance.SetBookUnlocked(questPanel.BookName, questPanel.Level);
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
        for (int i=0; i<questPanels.Count; i++)
        {
            if (questPanels[i].activeSelf)
            {
                if(i == questPanels.Count - 1)
                {
                    questPanels[i].SetActive(false);
                    break;
                }
                else if (questPanels[i+1].activeSelf)
                {
                    var tempBookName = questPanels[i+1].GetComponent<QuestPanel>().BookName;
                    questPanels[i].GetComponent<QuestPanel>().BookName = tempBookName;
                    var tempLevel = questPanels[i+1].GetComponent<QuestPanel>().Level;
                    questPanels[i].GetComponent<QuestPanel>().Level = tempLevel;
                    var tempValue = questPanels[i+1].GetComponentInChildren<Slider>().value;
                    questPanels[i].GetComponentInChildren<Slider>().value = tempValue;
                    var tempQuestPanel = questPanels[i].GetComponent<QuestPanel>();
                    var tempSlider= questPanels[i].GetComponentInChildren<Slider>();
                    StartCoroutine(QuestPanelStart(tempQuestPanel, tempSlider));
                }
                else
                {
                    if(i==0)
                    {
                        HasQuest = false;
                    }
                    questPanels[i].SetActive(false);
                    break;
                }
            }
        } 
    }
}
