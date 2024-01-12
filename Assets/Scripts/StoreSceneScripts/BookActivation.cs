using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class BookObjects : SerializableDictionary<string, List<GameObject>>{}
public class BookActivation : MonoBehaviour
{
#region Book Objects
    [Header("Book Objects Dictionary")]
    public List<GameObject> PassiveBookObjects = new List<GameObject>();
    public BookObjects EuropeBookObjects = new BookObjects();
    public BookObjects AsiaBookObjects = new BookObjects();
    public BookObjects NorthAmericaBookObjects = new BookObjects();
    public BookObjects SouthAmericaBookObjects = new BookObjects();
    public BookObjects AfricaBookObjects = new BookObjects();
    public BookObjects AustraliaBookObjects = new BookObjects();
#endregion

#region Legacy
    [Header("Legacy")]
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
#endregion

    void Start()
    {
        ActivateBook();
        ActivateBookObjects();
    }

    private void ActivateBookObjects(){
        foreach(string Boss in BookManager.Instance.EuropeBoss){
            EuropeBookObjects.Add(Boss, null);
            EuropeBookObjects[Boss] = new List<GameObject> {};
        }
        foreach(string Boss in BookManager.Instance.AsiaBoss){
            AsiaBookObjects.Add(Boss, null);
            AsiaBookObjects[Boss] = new List<GameObject> {};
        }
        foreach(string Boss in BookManager.Instance.NorthAmericaBoss){
            NorthAmericaBookObjects.Add(Boss, null);
            NorthAmericaBookObjects[Boss] = new List<GameObject> {};
        }
        foreach(string Boss in BookManager.Instance.SouthAmericaBoss){
            SouthAmericaBookObjects.Add(Boss, null);
            SouthAmericaBookObjects[Boss] = new List<GameObject> {};
        }
        foreach(string Boss in BookManager.Instance.AfricaBoss){
            AfricaBookObjects.Add(Boss, null);
            AfricaBookObjects[Boss] = new List<GameObject> {};
        }
        foreach(string Boss in BookManager.Instance.AustraliaBoss){
            AustraliaBookObjects.Add(Boss, null);
            AustraliaBookObjects[Boss] = new List<GameObject> {};
        }
    }

    [SerializeField] private GameObject location;
    [SerializeField] private GameObject bookObject;
    private void ActivateBook()
    {
        location = gameObject.transform.Find(BookManager.Instance.roomTypeSetting).gameObject;

        foreach (string Book in BookManager.Instance.TotalBookList){
            GameObject.Find(Book).SetActive(false);
        }

        switch (BookManager.Instance.roomTypeSetting){
            case "Europe":
                foreach(string Boss in BookManager.Instance.EuropeBoss){
                    foreach(string Book in BookManager.Instance.EuropeBookList[Boss]){
                        bookObject = location.transform.Find(Book).gameObject;
                        EuropeBookObjects[Boss].Add(bookObject);
                        if(BookManager.Instance.CheckBookUnlocked(Book) > 0){
                            bookObject.SetActive(true);
                        }
                        else{
                            bookObject.SetActive(false);
                        }
                    }
                }
                break;
            case "Asia":
                foreach(string Boss in BookManager.Instance.AsiaBoss){
                    foreach(string Book in BookManager.Instance.AsiaBookList[Boss]){
                        bookObject = location.transform.Find(Book).gameObject;
                        AsiaBookObjects[Boss].Add(bookObject);
                        if(BookManager.Instance.CheckBookUnlocked(Book) > 0){
                            bookObject.SetActive(true);
                        }
                        else{
                            bookObject.SetActive(false);
                        }
                    }
                }
                break;
            case "NorthAmerica":
                foreach(string Boss in BookManager.Instance.NorthAmericaBoss){
                    foreach(string Book in BookManager.Instance.NorthAmericaBookList[Boss]){
                        bookObject = location.transform.Find(Book).gameObject;
                        NorthAmericaBookObjects[Boss].Add(bookObject);
                        if(BookManager.Instance.CheckBookUnlocked(Book) > 0){
                            bookObject.SetActive(true);
                        }
                        else{
                            bookObject.SetActive(false);
                        }
                    }
                }
                break;
            case "SouthAmerica":
                foreach(string Boss in BookManager.Instance.SouthAmericaBoss){
                    foreach(string Book in BookManager.Instance.SouthAmericaBookList[Boss]){
                        bookObject = location.transform.Find(Book).gameObject;
                        SouthAmericaBookObjects[Boss].Add(bookObject);
                        if(BookManager.Instance.CheckBookUnlocked(Book) > 0){
                            bookObject.SetActive(true);
                        }
                        else{
                            bookObject.SetActive(false);
                        }
                    }
                }
                break;
            case "Africa":
                foreach(string Boss in BookManager.Instance.AfricaBoss){
                    foreach(string Book in BookManager.Instance.AfricaBookList[Boss]){
                        bookObject = location.transform.Find(Book).gameObject;
                        AfricaBookObjects[Boss].Add(bookObject);
                        if(BookManager.Instance.CheckBookUnlocked(Book) > 0){
                            bookObject.SetActive(true);
                        }
                        else{
                            bookObject.SetActive(false);
                        }
                    }
                }
                break;
            case "Australia":
                foreach(string Boss in BookManager.Instance.AustraliaBoss){
                    foreach(string Book in BookManager.Instance.AustraliaBookList[Boss]){
                        bookObject = location.transform.Find(Book).gameObject;
                        AustraliaBookObjects[Boss].Add(bookObject);
                        if(BookManager.Instance.CheckBookUnlocked(Book) > 0){
                            bookObject.SetActive(true);
                        }
                        else{
                            bookObject.SetActive(false);
                        }
                    }
                }
                break;
            case "Passive":
                foreach(string Passive in BookManager.Instance.PassiveList){
                    foreach(string Book in BookManager.Instance.PassiveBookList[Passive]){
                        bookObject = location.transform.Find(Book).gameObject;
                        PassiveBookObjects.Add(bookObject);
                        if(BookManager.Instance.CheckBookUnlocked(Book) > 0){
                            bookObject.SetActive(true);
                        }
                        else{
                            bookObject.SetActive(false);
                        }
                    }
                }
                break;
        }
    }

}

