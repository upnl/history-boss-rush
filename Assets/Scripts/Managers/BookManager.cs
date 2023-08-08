using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookManager : MonoBehaviour
{
    public static BookManager Instance { get; private set; }

    public string[] bookList;
    private BookData _bookData = new BookData();

    static public int Blood = 0;
    public int maxBlood = 200;

    public CSVReader bookDB;
    public CSVReader dialogueDB;
    public int iteration;

    private bool _thorDefeated = false;
    private bool _surtrDefeated = false;

    public bool thorDefeated => _thorDefeated;
    public bool surtrDefeated => _surtrDefeated;

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
        bookList = new string[] {"Surtr1", "Surtr2", "Surtr3", "Surtr4", "Surtr5", "Thor1", "Thor2", "Thor3", "Thor4"};

        bookDB = new CSVReader(_bookDB, true, '\t');
        dialogueDB = new CSVReader(_dialogueDB, true, ',');
        iteration = 1;

        bookDescription = "";
    }

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
        foreach(string bookName in bookList)
        {
            _bookData.bookUnlocked[bookName] = 0;
        }
    }

    public void ResetBookEquipped()
    {
        foreach(string bookName in bookList)
        {
            _bookData.bookEquipped[bookName] = 0;
        }
    }
    
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
    

    public void BossDefeated(string bossName)
    {
        ResetBookUnlocked();
        if (bossName == "Thor")
        {
            _thorDefeated = true;
        }
        if (bossName == "Surtr")
        {
            _surtrDefeated = true;
        }
    }
}
