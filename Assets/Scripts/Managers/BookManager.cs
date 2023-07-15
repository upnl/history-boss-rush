using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookManager : MonoBehaviour
{
    public static BookManager Instance { get; private set; }

    public string[] bookList;
    private BookData _bookData = new BookData();
    private float _blood;
    public float blood => _blood;
    public CSVReader bookDB;

    [SerializeField] private TextAsset _bookDB;

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
        _blood = 100f;

        bookDB = new CSVReader(_bookDB, true, '\t');
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

    public void SetBookEquipped(string bookName, int level, float price)
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
}
