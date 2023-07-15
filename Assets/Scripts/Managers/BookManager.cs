using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookManager : MonoBehaviour
{
    public static BookManager Instance { get; private set; }

    public string[] bookList = new string[] {"judgment", "challenge", "tenacity", "alertness"};
    private BookData _bookData = new BookData();
    public int blood = 10000;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    public int checkBookUnlocked(string bookName)
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

    public int checkBookEquipped(string bookName)
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

    public void setBookUnlocked(string bookName, int level)
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

    public void setBookEquipped(string bookName, int level, int price)
    {
        try
        {
            if (_bookData.bookEquipped[bookName] < level)
            {
                _bookData.bookEquipped[bookName] = level;
                blood -= price;
            }
        }
        catch (KeyNotFoundException)
        {
            _bookData.bookEquipped[bookName] = level;
            blood -= price;
        }
    }
}
