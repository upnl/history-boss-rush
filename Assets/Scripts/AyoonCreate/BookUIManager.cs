using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookUIManager : MonoBehaviour
{
    public static BookUIManager Instance { get; private set; }

    private int _iteration = 0;

    private bool _b1 = false;
    private bool _b2 = false;
    private bool _b3 = false;
    private bool _b4 = false;
    private bool _b5 = false;
    private bool _b6 = false;
    private bool _b7 = false;
    private bool _b8 = false;
    private bool _b9 = false;
    private bool _b10 = false;
    private bool _b11 = false;
    private bool _b12 = false;
    private bool _b13 = false;
    private bool _b14 = false;
    private bool _b15 = false;
    private bool _b16 = false;
    private bool _b17 = false;
    private bool _b18 = false;
    private bool _b19 = false;
    private bool _b20 = false;
    private bool _b21 = false;
    private bool _b22 = false;

    public GameObject parent;

    public GameObject[] Thor1;
    public GameObject[] Thor2;
    public GameObject[] Thor3;
    public GameObject[] Thor4;

    public GameObject[] Surtur1;
    public GameObject[] Surtur2;
    public GameObject[] Surtur3;
    public GameObject[] Surtur4;

    public GameObject[] God31;
    public GameObject[] God32;
    public GameObject[] God33;
    public GameObject[] God34;

    public GameObject[] God41;
    public GameObject[] God42;
    public GameObject[] God43;
    public GameObject[] God44;

    public GameObject[] Cha;
    public GameObject[] Ten;
    public GameObject[] Ale;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        ShowBookUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowBookUI()
    {
        switch (BookManager.Instance.CheckBookEquipped("Thor1"))
        {
            case 0:
                break;
            case 1:
                if (_b1 == false)
                {
                    var _bookUI1 = Instantiate(Thor1[0], new Vector3(30 - (50 * _iteration), 0f, 0f), Quaternion.identity);
                    _bookUI1.transform.SetParent(parent.transform, false);
                    _iteration++;
                    _b1 = true;
                }
                break;
            case 2:
                if (_b2 == false)
                {
                    var bookUI2 = Instantiate(Thor1[1], new Vector3(30 - (50 * _iteration), 0f, 0f), Quaternion.identity);
                    bookUI2.transform.SetParent(parent.transform, false);
                    _iteration++;
                    _b2 = true;
                }
                break;
        }

        switch (BookManager.Instance.CheckBookEquipped("Thor2"))
        {
            case 0:
                break;
            case 1:
                if (_b3 == false)
                {
                    var _bookUI3 = Instantiate(Thor2[0], new Vector3(30 - (50 * _iteration), 0f, 0f), Quaternion.identity);
                    _bookUI3.transform.SetParent(parent.transform, false);
                    _iteration++;
                    _b3 = true;
                }
                break;
            case 2:
                if (_b4 == false)
                {
                    var _bookUI4 = Instantiate(Thor2[1], new Vector3(30 - (50 * _iteration), 0f, 0f), Quaternion.identity);
                    _bookUI4.transform.SetParent(parent.transform, false);
                    _iteration++;
                    _b4 = true;
                }
                break;
        }

        switch (BookManager.Instance.CheckBookEquipped("Thor3"))
        {
            case 0:
                break;
            case 1:
                if (_b5 == false)
                {
                    var _bookUI5 = Instantiate(Thor3[0], new Vector3(30 - (50 * _iteration), 0f, 0f), Quaternion.identity);
                    _bookUI5.transform.SetParent(parent.transform, false);
                    _iteration++;
                    _b5 = true;
                }
                break;
            case 2:
                if (_b6 == false)
                {
                    var _bookUI6 = Instantiate(Thor3[1], new Vector3(30 - (50 * _iteration), 0f, 0f), Quaternion.identity);
                    _bookUI6.transform.SetParent(parent.transform, false);
                    _iteration++;
                    _b6 = true;
                }
                break;
        }

        switch (BookManager.Instance.CheckBookEquipped("Thor4"))
        {
            case 0:
                break;
            case 1:
                if (_b7 == false)
                {
                    var _bookUI7 = Instantiate(Thor4[0], new Vector3(30 - (50 * _iteration), 0f, 0f), Quaternion.identity);
                    _bookUI7.transform.SetParent(parent.transform, false);
                    _iteration++;
                    _b7 = true;
                }
                break;
            case 2:
                if (_b8 == false)
                {
                    var _bookUI8 = Instantiate(Thor4[1], new Vector3(30 - (50 * _iteration), 0f, 0f), Quaternion.identity);
                    _bookUI8.transform.SetParent(parent.transform, false);
                    _iteration++;
                    _b8 = true;
                }
                break;
        }

        switch (BookManager.Instance.CheckBookEquipped("Surtr1"))
        {
            case 0:
                break;
            case 1:
                if (_b9 == false)
                {
                    var _bookUI9 = Instantiate(Surtur1[0], new Vector3(30 - (50 * _iteration), 0f, 0f), Quaternion.identity);
                    _bookUI9.transform.SetParent(parent.transform, false);
                    _iteration++;
                    _b9 = true;
                }
                break;
            case 2:
                if (_b10 == false)
                {
                    var _bookUI10 = Instantiate(Surtur1[1], new Vector3(30 - (50 * _iteration), 0f, 0f), Quaternion.identity);
                    _bookUI10.transform.SetParent(parent.transform, false);
                    _iteration++;
                    _b10 = true;
                }
                break;
        }

        switch (BookManager.Instance.CheckBookEquipped("Surtr2"))
        {
            case 0:
                break;
            case 1:
                if (_b11 == false)
                {
                    var _bookUI11 = Instantiate(Surtur2[0], new Vector3(30 - (50 * _iteration), 0f, 0f), Quaternion.identity);
                    _bookUI11.transform.SetParent(parent.transform, false);
                    _iteration++;
                    _b11 = true;
                }
                break;
            case 2:
                if (_b12 == false)
                {
                    var _bookUI12 = Instantiate(Surtur2[1], new Vector3(30 - (50 * _iteration), 0f, 0f), Quaternion.identity);
                    _bookUI12.transform.SetParent(parent.transform, false);
                    _iteration++;
                    _b12 = true;
                }
                break;
        }

        switch (BookManager.Instance.CheckBookEquipped("Surtr3"))
        {
            case 0:
                break;
            case 1:
                if (_b13 == false)
                {
                    var _bookUI13 = Instantiate(Surtur3[0], new Vector3(30 - (50 * _iteration), 0f, 0f), Quaternion.identity);
                    _bookUI13.transform.SetParent(parent.transform, false);
                    _iteration++;
                    _b13 = true;
                }
                break;
            case 2:
                if (_b14 == false)
                {
                    var _bookUI14 = Instantiate(Surtur3[1], new Vector3(30 - (50 * _iteration), 0f, 0f), Quaternion.identity);
                    _bookUI14.transform.SetParent(parent.transform, false);
                    _iteration++;
                    _b14 = true;
                }
                break;
        }

        switch (BookManager.Instance.CheckBookEquipped("Surtr4"))
        {
            case 0:
                break;
            case 1:
                if (_b15 == false)
                {
                    var _bookUI15 = Instantiate(Surtur4[0], new Vector3(30 - (50 * _iteration), 0f, 0f), Quaternion.identity);
                    _bookUI15.transform.SetParent(parent.transform, false);
                    _iteration++;
                    _b15 = true;
                }
                break;
            case 2:
                if (_b16 == false)
                {
                    var _bookUI16 = Instantiate(Surtur4[1], new Vector3(30 - (50 * _iteration), 0f, 0f), Quaternion.identity);
                    _bookUI16.transform.SetParent(parent.transform, false);
                    _iteration++;
                    _b16 = true;
                }
                break;
        }

        switch (BookManager.Instance.CheckBookEquipped("Challenge"))
        {
            case 0:
                break;
            case 1:
                if (_b17 == false)
                {
                    var _bookUI17 = Instantiate(Cha[0], new Vector3(30 - (50 * _iteration), 0f, 0f), Quaternion.identity);
                    _bookUI17.transform.SetParent(parent.transform, false);
                    _iteration++;
                    _b17 = true;
                }
                break;
            case 2:
                if (_b18 == false)
                {
                    var _bookUI18 = Instantiate(Cha[1], new Vector3(30 - (50 * _iteration), 0f, 0f), Quaternion.identity);
                    _bookUI18.transform.SetParent(parent.transform, false);
                    _iteration++;
                    _b18 = true;
                }
                break;
        }

        switch (BookManager.Instance.CheckBookEquipped("Tenacity"))
        {
            case 0:
                break;
            case 1:
                if (_b19 == false)
                {
                    var _bookUI19 = Instantiate(Ten[0], new Vector3(30 - (50 * _iteration), 0f, 0f), Quaternion.identity);
                    _bookUI19.transform.SetParent(parent.transform, false);
                    _iteration++;
                    _b19 = true;
                }
                break;
            case 2:
                if (_b20 == false)
                {
                    var _bookUI20 = Instantiate(Ten[1], new Vector3(30 - (50 * _iteration), 0f, 0f), Quaternion.identity);
                    _bookUI20.transform.SetParent(parent.transform, false);
                    _iteration++;
                    _b20 = true;
                }
                break;
        }

        switch (BookManager.Instance.CheckBookEquipped("Alertness"))
        {
            case 0:
                break;
            case 1:
                if (_b21 == false)
                {
                    var _bookUI21 = Instantiate(Ale[0], new Vector3(30 - (50 * _iteration), 0f, 0f), Quaternion.identity);
                    _bookUI21.transform.SetParent(parent.transform, false);
                    _iteration++;
                    _b21 = true;
                }
                break;
            case 2:
                if (_b22 == false)
                {
                    var _bookUI22 = Instantiate(Ale[1], new Vector3(30 - (50 * _iteration), 0f, 0f), Quaternion.identity);
                    _bookUI22.transform.SetParent(parent.transform, false);
                    _iteration++;
                    _b22 = true;
                }
                break;
        }
    }
}
