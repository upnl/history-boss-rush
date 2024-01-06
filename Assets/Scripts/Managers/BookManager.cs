using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookManager : MonoBehaviour
{
    public static BookManager Instance { get; private set; }

    public string roomTypeSetting;

#region Book List Declaration
    public List<string> TotalBookList;
    public Dictionary<string, List<string>> EuropeBookList;
    public Dictionary<string, List<string>> AsiaBookList;
    public Dictionary<string, List<string>> NorthAmericaBookList;
    public Dictionary<string, List<string>> SouthAmericaBookList;
    public Dictionary<string, List<string>> AfricaBookList;
    public Dictionary<string, List<string>> AustraliaBookList;
#endregion

    private BookData _bookData = new BookData();

    public int Blood = 0;
    public int maxBlood = 200;

    public CSVReader bookDB;
    public CSVReader dialogueDB;
    public int iteration;

    private bool _thorDefeated = false;
    private bool _surtrDefeated = false;
    private bool _hellDefeated = false;
    private bool _lokiDefeated = false;

    public bool thorDefeated => _thorDefeated;
    public bool surtrDefeated => _surtrDefeated;
    public bool hellDefeated => _hellDefeated;
    public bool lokiDefeated => _lokiDefeated;

    [SerializeField] private TextAsset _bookDB;
    [SerializeField] private TextAsset _dialogueDB;

    public string bookDescription;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(this.gameObject);
        TotalBookList = new List<string> {"Surtr1", "Surtr2", "Surtr3", "Surtr4", "Surtr5", "Thor1", "Thor2", "Thor3", "Thor4"};

        EuropeBookList["Surtr"] = new List<string> {"Surtr1", "Surtr2", "Surtr3", "Surtr4", "Surtr5"};
        EuropeBookList["Thor"] = new List<string> {"Thor1", "Thor2", "Thor3", "Thor4"};

        bookDB = new CSVReader(_bookDB, true, '\t');
        dialogueDB = new CSVReader(_dialogueDB, true, ',');
        iteration = 1;

        bookDescription = "";
    }

#region Room Setting
    public void SetRoomPassive(){
        roomTypeSetting = "Passive";
    }
    public void SetRoomEurope(){
        roomTypeSetting = "Europe";
    }
    public void SetRoomAsia(){
        roomTypeSetting = "Asia";
    }
    public void SetRoomNorthAmerica(){
        roomTypeSetting = "NorthAmerica";
    }
    public void SetRoomSouthAmerica(){
        roomTypeSetting = "SouthAmerica";
    }
    public void SetRoomAfrica(){
        roomTypeSetting = "Africa";
    }
    public void SetRoomAustralia(){
        roomTypeSetting = "Australia";
    }
    


#endregion


#region Book Unlock / Equip
    public int CheckBookUnlocked(string bookName)
    {
        try
        {
            return _bookData.bookUnlocked[bookName];
        }
        catch (KeyNotFoundException)
        {
            _bookData.bookUnlocked[bookName] = 0;
            return 0;
        }
    }

    public int CheckBookEquipped(string bookName)
    {
        try
        {
            return _bookData.bookEquipped[bookName];
        }
        catch (KeyNotFoundException)
        {
            _bookData.bookEquipped[bookName] = 0;
            return 0;
        }

    }

    public void SetBookUnlocked(string bookName, int level)
    {
        try
        {
            if (_bookData.bookUnlocked[bookName] < level)
            {
                _bookData.bookUnlocked[bookName] = level;
            }
        }
        catch (KeyNotFoundException)
        {
            _bookData.bookUnlocked[bookName] = level;
        }
    }
    
    public void SetBookEquipped(string bookName, int level, int price)
    {
        if (price > Blood)
        {
            return;
        }
        try
        {
            if (_bookData.bookEquipped[bookName] < level)
            {
                _bookData.bookEquipped[bookName] = level;
                Blood -= price;
            }
        }
        catch (KeyNotFoundException)
        {
            _bookData.bookEquipped[bookName] = level;
            Blood -= price;
        }
    }
    

    public void ResetBookUnlocked()
    {
        foreach(string bookName in TotalBookList)
        {
            _bookData.bookUnlocked[bookName] = 0;
        }
    }

    public void ResetBookEquipped()
    {
        foreach(string bookName in TotalBookList)
        {
            _bookData.bookEquipped[bookName] = 0;
        }
    }
    
#endregion


#region Blood
    public void AddBlood(int amount)
    {
        if (Blood < maxBlood)
        {
            Blood += amount;
        }
        else
        {
            Blood = maxBlood;
        }
    }
#endregion    


#region Boss Defeat
    public void BossDefeated(string bossName)
    {
        if (bossName == "Thor")
        {
            _thorDefeated = true;
        }

        if (bossName == "Surtr")
        {
            _surtrDefeated = true;
        }

        if (bossName == "Hell")
        {
            _hellDefeated = true;
        }

        if (bossName == "Loki")
        {
            _lokiDefeated = true;
        }
    }

    public int CheckBossDefeated(string bossName)
    {
        if (bossName == "Thor")
        {
            if (thorDefeated)
            {
                return 1;
            }

            else
            {
                return 0;
            }
        }

        else if (bossName == "Surtr")
        {
            if (surtrDefeated)
            {
                return 1;
            }

            else
            {
                return 0;
            }
        }

        else if (bossName == "Hell")
        {
            if (hellDefeated)
            {
                return 1;
            }

            else
            {
                return 0;
            }
        }

        else if (bossName == "Loki")
        {
            if (lokiDefeated)
            {
                return 1;
            }

            else
            {
                return 0;
            }
        }

        else
        {
            return 0;
        }
    }
#endregion

}