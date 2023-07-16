using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookManager : MonoBehaviour
{
    public static BookManager Instance { get; private set; }

    public string[] bookList;
    private BookData _bookData = new BookData();
    private int _blood;
    public int blood => _blood;

    private int _baseBlood = 15;

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
        bookList = new string[] {"Surtr1", "Surtr2", "Surtr3", "Surtr4", "Surtr5", "Thor1", "Thor2", "Thor3", "Thor4", "Challenge", "Tenacity", "Alertness"};
        _blood = _baseBlood;

        bookDB = new CSVReader(_bookDB, true, '\t');
        dialogueDB = new CSVReader(_dialogueDB, true, ',');
        iteration = 1;

        bookDescription = "연구할 역사책을 선택하자";
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
        if (price > _blood)
        {
            return;
        }
        try
        {
            if (_bookData.bookEquipped[bookName] < level)
            {
                _bookData.bookEquipped[bookName] = level;
                _blood -= price;
            }
        }
        catch (KeyNotFoundException)
        {
            _bookData.bookEquipped[bookName] = level;
            _blood -= price;
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

    public void ResetBlood()
    {
        _blood = _baseBlood;
    }

    public void AddBlood(int amount)
    {
        _blood += amount;
    }

    public void BossDefeated(string bossName)
    {
        ResetBookUnlocked();
        if (bossName == "thor")
        {
            _thorDefeated = true;
        }
        if (bossName == "surtr")
        {
            _surtrDefeated = true;
        }
    }
}
