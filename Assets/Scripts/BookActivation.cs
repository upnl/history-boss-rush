using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class BookActivation : MonoBehaviour
{
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

    void Start()
    {
        for (int i = 1; i < 5; i ++)
        {
            for (int j = 0; j < 8; j++)
            {
                transform.GetChild(i).gameObject.transform.GetChild(j).gameObject.SetActive(false);
            }
        }
    }

    private void ActivateBook()
    {
        switch (BookManager.Instance.CheckBookUnlocked("Thor1"))
        {
            case 0:
                break;
            case 1:
                Thor1[0].gameObject.SetActive(true);
                break;
            case 2:
                Thor1[0].gameObject.SetActive(true);
                Thor1[1].gameObject.SetActive(true);
                break;
        }

        switch (BookManager.Instance.CheckBookUnlocked("Thor2"))
        {
            case 0:
                break;
            case 1:
                Thor2[0].gameObject.SetActive(true);
                break;
            case 2:
                Thor2[0].gameObject.SetActive(true);
                Thor2[1].gameObject.SetActive(true);
                break;
        }

        switch (BookManager.Instance.CheckBookUnlocked("Thor3"))
        {
            case 0:
                break;
            case 1:
                Thor3[0].gameObject.SetActive(true);
                break;
            case 2:
                Thor3[0].gameObject.SetActive(true);
                Thor3[1].gameObject.SetActive(true);
                break;
        }

        switch (BookManager.Instance.CheckBookUnlocked("Thor4"))
        {
            case 0:
                break;
            case 1:
                Thor4[0].gameObject.SetActive(true);
                break;
            case 2:
                Thor4[0].gameObject.SetActive(true);
                Thor4[1].gameObject.SetActive(true);
                break;
        }

        switch (BookManager.Instance.CheckBookUnlocked("Surtr1"))
        {
            case 0:
                break;
            case 1:
                Surtur1[0].gameObject.SetActive(true);
                break;
            case 2:
                Surtur1[0].gameObject.SetActive(true);
                Surtur1[1].gameObject.SetActive(true);
                break;
        }

        switch (BookManager.Instance.CheckBookUnlocked("Surtr2"))
        {
            case 0:
                break;
            case 1:
                Surtur2[0].gameObject.SetActive(true);
                break;
            case 2:
                Surtur2[0].gameObject.SetActive(true);
                Surtur2[1].gameObject.SetActive(true);
                break;
        }

        switch (BookManager.Instance.CheckBookUnlocked("Surtr3"))
        {
            case 0:
                break;
            case 1:
                Surtur3[0].gameObject.SetActive(true);
                break;
            case 2:
                Surtur3[0].gameObject.SetActive(true);
                Surtur3[1].gameObject.SetActive(true);
                break;
        }

        switch (BookManager.Instance.CheckBookUnlocked("Surtr4"))
        {
            case 0:
                break;
            case 1:
                Surtur4[0].gameObject.SetActive(true);
                break;
            case 2:
                Surtur4[0].gameObject.SetActive(true);
                Surtur4[1].gameObject.SetActive(true);
                break;
        }
    }
   

}
