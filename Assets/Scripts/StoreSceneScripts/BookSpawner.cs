using UnityEngine;
using Random = UnityEngine.Random;
using System;
using System.Collections.Generic;

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

    private List<List<float>> _bookLocations = new List<List<float>>();


    private void Start()
    {
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
        BookManager.Instance.setBookUnlocked("judgment", 2);
        BookManager.Instance.setBookUnlocked("tenacity", 1);
        SpawnRealBooks();
    }

    private void SpawnRealBooks()
    {
        foreach(string bookName in BookManager.Instance.bookList)
        {
            if (BookManager.Instance.checkBookUnlocked(bookName) >= 2)
            {
                var book = SpawnBook(BookType.RealTwo);
                book.GetComponent<BookBehaviour>().setProperties(bookName, 2, 20);

                book = SpawnBookClosely(BookType.RealOne, book.transform.position);
                book.GetComponent<BookBehaviour>().setProperties(bookName, 1, 10);
            }
            else if (BookManager.Instance.checkBookUnlocked(bookName) >= 1)
            {
                var book = SpawnBook(BookType.RealOne);
                book.GetComponent<BookBehaviour>().setProperties(bookName, 1, 10);
            }
        }
    }

    private GameObject SpawnBook(BookType bookType)
    {
        var prefab = returnPrefab(bookType);
        var position = spawnPosition(bookType);
        var book = Instantiate<GameObject>(prefab, position, Quaternion.identity);

        return book;
    }

    private GameObject SpawnBookClosely(BookType bookType, Vector3 previousPosition)
    {
        var prefab = returnPrefab(bookType);
        var position = previousPosition;
        position.x -= 0.5f;
        var book = Instantiate<GameObject>(prefab, position, Quaternion.identity);

        return book;
    }

    private Vector3 spawnPosition(BookType bookType)
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

    private GameObject returnPrefab(BookType bookType)
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
