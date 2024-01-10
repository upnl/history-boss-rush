using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookManager : MonoBehaviour
{
    public static BookManager Instance { get; private set; }

    public string roomTypeSetting;

#region Book List Declaration
    public List<string> PassiveList = new List<string>{};

    public List<string> TotalBookList = new List<string> {};
    public Dictionary<string, List<string>> PassiveBookList = new Dictionary<string, List<string>>();
    public Dictionary<string, List<string>> EuropeBookList = new Dictionary<string, List<string>>();
    public Dictionary<string, List<string>> AsiaBookList = new Dictionary<string, List<string>>();
    public Dictionary<string, List<string>> NorthAmericaBookList = new Dictionary<string, List<string>>();
    public Dictionary<string, List<string>> SouthAmericaBookList = new Dictionary<string, List<string>>();
    public Dictionary<string, List<string>> AfricaBookList = new Dictionary<string, List<string>>();
    public Dictionary<string, List<string>> AustraliaBookList = new Dictionary<string, List<string>>();
#endregion

#region Boss List Declaration
    [Header("Boss List")]
    public List<string> TotalBoss = new List<string> {};
    public List<string> EuropeBoss = new List<string>();
    public List<string> AsiaBoss = new List<string>();
    public List<string> NorthAmericaBoss = new List<string>();
    public List<string> SouthAmericaBoss = new List<string>();
    public List<string> AfricaBoss = new List<string>();
    public List<string> AustraliaBoss = new List<string>();
#endregion

    private BookData _bookData = new BookData();

    public int Blood = 0;
    public int maxBlood = 200;

    public CSVReader bookDB;
    public CSVReader dialogueDB;
    public int iteration;

    private Dictionary<string, bool> _bossDefeated = new Dictionary<string, bool>();
    public Dictionary<string, bool> bossDefeated => _bossDefeated;

    [SerializeField] private TextAsset _bookDB;
    [SerializeField] private TextAsset _dialogueDB;

    public string bookDescription;


#region Initiate
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(this.gameObject);

        initiateBookList();
        initiateBossDefeated();

        bookDB = new CSVReader(_bookDB, true, '\t');
        dialogueDB = new CSVReader(_dialogueDB, true, ',');
        iteration = 1;

        bookDescription = "";
    }

    private int i, j;
    private string iStr, jStr, bookName;
    private void initiateBookList(){
        foreach (string Boss in EuropeBoss){
            TotalBoss.Add(Boss);
            EuropeBookList[Boss] = new List<string> {};
            for(i = 1; i <= 4; i++){
                for(j = 1; j <= 2; j++){
                    iStr = i.ToString();
                    jStr = j.ToString();
                    bookName = $"{Boss} {iStr}-{jStr}";
                    EuropeBookList[Boss].Add(bookName);
                    TotalBookList.Add(bookName);
                } 
            }
        }
        foreach (string Boss in AsiaBoss){
            TotalBoss.Add(Boss);
            AsiaBookList[Boss] = new List<string> {};
            for(i = 1; i <= 4; i++){
                for(j = 1; j <= 2; j++){
                    iStr = i.ToString();
                    jStr = j.ToString();
                    bookName = $"{Boss} {iStr}-{jStr}";
                    AsiaBookList[Boss].Add(bookName);
                    TotalBookList.Add(bookName);
                } 
            }
        }
        foreach (string Boss in NorthAmericaBoss){
            TotalBoss.Add(Boss);
            NorthAmericaBookList[Boss] = new List<string> {};
            for(i = 1; i <= 4; i++){
                for(j = 1; j <= 2; j++){
                    iStr = i.ToString();
                    jStr = j.ToString();
                    bookName = $"{Boss} {iStr}-{jStr}";
                    NorthAmericaBookList[Boss].Add(bookName);
                    TotalBookList.Add(bookName);
                } 
            }
        }
        foreach (string Boss in SouthAmericaBoss){
            TotalBoss.Add(Boss);
            SouthAmericaBookList[Boss] = new List<string> {};
            for(i = 1; i <= 4; i++){
                for(j = 1; j <= 2; j++){
                    iStr = i.ToString();
                    jStr = j.ToString();
                    bookName = $"{Boss} {iStr}-{jStr}";
                    SouthAmericaBookList[Boss].Add(bookName);
                    TotalBookList.Add(bookName);
                } 
            }
        }
        foreach (string Boss in AfricaBoss){
            TotalBoss.Add(Boss);
            AfricaBookList[Boss] = new List<string> {};
            for(i = 1; i <= 4; i++){
                for(j = 1; j <= 2; j++){
                    iStr = i.ToString();
                    jStr = j.ToString();
                    bookName = $"{Boss} {iStr}-{jStr}";
                    AfricaBookList[Boss].Add(bookName);
                    TotalBookList.Add(bookName);
                } 
            }
        }
        foreach (string Boss in AustraliaBoss){
            TotalBoss.Add(Boss);
            AustraliaBookList[Boss] = new List<string> {};
            for(i = 1; i <= 4; i++){
                for(j = 1; j <= 2; j++){
                    iStr = i.ToString();
                    jStr = j.ToString();
                    bookName = $"{Boss} {iStr}-{jStr}";
                    AustraliaBookList[Boss].Add(bookName);
                    TotalBookList.Add(bookName);
                } 
            }
        }
        foreach (string Passive in PassiveList){
            PassiveBookList[Passive] = new List<string> {};
            for(i = 1; i <= 2; i++){
                iStr = i.ToString();
                bookName = $"{Passive} {iStr}";
                PassiveBookList[Passive].Add(bookName);
                TotalBookList.Add(bookName);
            }
        }

    }

    private void initiateBossDefeated(){
        foreach (string Boss in TotalBoss){
            _bossDefeated[Boss] = false;
        }
    }
#endregion


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
        _bossDefeated[bossName] = true;
    }

    public int CheckBossDefeated(string bossName)
    {
        if(_bossDefeated[bossName]) return 1;
        else return 0;
    }
#endregion

}