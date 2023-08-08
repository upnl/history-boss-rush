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

    [SerializeField] private GameObject _realBookOne;
    [SerializeField] private GameObject _realBookTwo;

    [SerializeField] private GameObject _dummyBook;
    [SerializeField] private GameObject _dummyBookLeftTilt;
    [SerializeField] private GameObject _dummyBookRightTilt;

    [SerializeField] private GameObject _dummyBookGroupOne;
    [SerializeField] private GameObject _dummyBookGroupTwo;

    private List<List<float>> _bookLocations = new List<List<float>>();

    private CSVReader _csv;
    private void Start()
    {
        _csv = BookManager.Instance.bookDB;
        for (int i = 0; i < 4; ++i)
        {
            _bookLocations.Add(new List<float>());
        }
        //For debugging purposes, two books are unlocked artificially
        /*
        BookManager.Instance.SetBookUnlocked("Tenacity", 1);
        BookManager.Instance.SetBookUnlocked("Challenge", 2);
        BookManager.Instance.SetBookUnlocked("Alertness", 2);
        BookManager.Instance.SetBookUnlocked("Surtr4", 2);
        BookManager.Instance.SetBookUnlocked("Thor3", 3);
        */
        BookManager.Instance.ResetBookEquipped();


        AudioManager.Instance.PlayBGM();
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

    private List<string> ScanCSVForRow(string bookName, int level)
    {
        var i = 0;
        while (i < _csv.GetData().Count)
        {
            if (_csv.GetData()[i][1] == bookName && Convert.ToInt32(_csv.GetData()[i][3]) == level)
            {
                return _csv.GetData()[i];
            }
            i += 1;
        }
        Debug.Log("book name and level not found in csv file");
        return new List<string>();
    }

    private string ParseSentence(string original, int rowIndex)
    {
        List<int> parenStart = new List<int>();
        List<int> parenEnd = new List<int>();

        var i = 0;
        while (i < original.Length)
        {
            if (Convert.ToString(original[i]) == "{")
            {
                parenStart.Add(i);
            }
            if (Convert.ToString(original[i]) == "}")
            {
                parenEnd.Add(i);
            }
            i += 1;
        }

        string newSentence = "";

        int previousEnd = 0;
        i = 0;
        while (i < parenStart.Count)
        {
            newSentence = newSentence + original.Substring(previousEnd, parenStart[i] - previousEnd);
            newSentence = newSentence + _csv.GetColumn(original.Substring(parenStart[i] + 1, parenEnd[i] - parenStart[i] - 1))[rowIndex];
            previousEnd = parenEnd[i] + 1;
            i += 1;
        }
        newSentence = newSentence + original.Substring(previousEnd);

        return newSentence;
    }

    

   
}
