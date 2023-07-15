using UnityEngine;
using Random = UnityEngine.Random;
using System;
using System.Collections.Generic;
using System.IO;

public enum BookType
{
    RealOne,
    RealTwo,
    Dummy,
    DummyLeft,
    DummyRight,

    DummyGroupOne,
    DummyGroupTwo
}

public class BookSpawner : MonoBehaviour
{

    [SerializeField] private ShelfData _shelfData;

    [SerializeField] private GameObject _realBookOne;
    [SerializeField] private GameObject _realBookTwo;

    [SerializeField] private GameObject _dummyBook;
    [SerializeField] private GameObject _dummyBookLeftTilt;
    [SerializeField] private GameObject _dummyBookRightTilt;

    [SerializeField] private GameObject _dummyBookGroupOne;
    [SerializeField] private GameObject _dummyBookGroupTwo;
    [SerializeField] private TextAsset _bookDB;

    private List<List<float>> _bookLocations = new List<List<float>>();

    private CSVReader _csv;
    private void Start()
    {
        _csv = new CSVReader(_bookDB, true, '\t');
        for (int i = 0; i < 4; ++i)
        {
            _bookLocations.Add(new List<float>());
        }
        for (int i = 0; i < 5; ++i)
        {
            SpawnBook(BookType.DummyLeft);
            SpawnBook(BookType.DummyRight);
        }
        for (int i = 0; i < 3; ++i)
        {
            SpawnBook(BookType.DummyGroupOne);
            SpawnBook(BookType.DummyGroupTwo);
        }
        //For debugging purposes, two books are unlocked artificially
        BookManager.Instance.SetBookUnlocked("Tenacity", 1);
        BookManager.Instance.SetBookUnlocked("Challenge", 2);
        BookManager.Instance.SetBookUnlocked("Alertness", 2);
        SpawnRealBooks();
    }

    private void SpawnRealBooks()
    {
        foreach(string bookName in BookManager.Instance.bookList)
        {
            if (BookManager.Instance.CheckBookUnlocked(bookName) >= 2)
            {
                var book = SpawnBook(BookType.RealTwo);
                book.GetComponent<BookBehaviour>().SetProperties(bookName, 2, ScanCSVForPrice(bookName, 2));

                book = SpawnBookClosely(BookType.RealOne, book.transform.position);
                book.GetComponent<BookBehaviour>().SetProperties(bookName, 1, ScanCSVForPrice(bookName, 1));
            }
            else if (BookManager.Instance.CheckBookUnlocked(bookName) >= 1)
            {
                var book = SpawnBook(BookType.RealOne);
                book.GetComponent<BookBehaviour>().SetProperties(bookName, 1, ScanCSVForPrice(bookName, 1));
            }
        }
    }

    private GameObject SpawnBook(BookType bookType)
    {
        var prefab = ReturnPrefab(bookType);
        var position = SpawnPosition(bookType);
        var book = Instantiate<GameObject>(prefab, position, Quaternion.identity);

        return book;
    }

    private GameObject SpawnBookClosely(BookType bookType, Vector3 previousPosition)
    {
        var prefab = ReturnPrefab(bookType);
        var position = previousPosition;
        position.x -= 0.5f;
        var book = Instantiate<GameObject>(prefab, position, Quaternion.identity);

        return book;
    }

    private float ScanCSVForPrice(string bookName, int level)
    {

        var i = 0;
        while (i < _csv.GetData().Count)
        {
            if (_csv.GetData()[i][1] == bookName && Convert.ToInt32(_csv.GetData()[i][3]) == level)
            {
                return Convert.ToSingle(_csv.GetData()[i][4]);
            }
            i += 1;
        }
        return 0f;
    }

    private Vector3 SpawnPosition(BookType bookType)
    {
        var overlap = true;
        var x = 0f;
        var y = 0f;
        var yCase = 0;
        while (overlap)
        {
            overlap = false;
            x = Random.Range(_shelfData.xMin, _shelfData.xMax);
            yCase = Random.Range(0,4);
            switch (yCase)
            {
                case 0:
                    y = _shelfData.yRow1;
                    break;
                case 1:
                    y = _shelfData.yRow2;
                    break;
                case 2:
                    y = _shelfData.yRow3;
                    break;
                case 3:
                    y = _shelfData.yRow4;
                    break;
                default:
                    break;
            }

            foreach (float location in _bookLocations[yCase])
            {
                if (x < location + _shelfData.overlapRange && x > location - _shelfData.overlapRange)
                {
                    overlap = true;
                }
            }
        }
        _bookLocations[yCase].Add(x);
        return new Vector3(x, y, 0f);
    }

    private GameObject ReturnPrefab(BookType bookType)
    {
        var prefab = _dummyBook;
        switch (bookType)
        {
            case BookType.RealOne:
                prefab = _realBookOne;
                break;
            case BookType.RealTwo:
                prefab = _realBookTwo;
                break;
            case BookType.Dummy:
                prefab = _dummyBook;
                break;
            case BookType.DummyLeft:
                prefab = _dummyBookLeftTilt;
                break;
            case BookType.DummyRight:
                prefab = _dummyBookRightTilt;
                break;
            case BookType.DummyGroupOne:
                prefab = _dummyBookGroupOne;
                break;
            case BookType.DummyGroupTwo:
                prefab = _dummyBookGroupTwo;
                break;
            default:
                prefab = _dummyBook;
                Debug.Log("return prefab error");
                break;
        }

        return prefab;
    }
}
