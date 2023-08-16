using UnityEngine;
using Random = UnityEngine.Random;
using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class testing : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BookManager.Instance.SetBookUnlocked("Thor1", 2);
        BookManager.Instance.SetBookUnlocked("Thor2", 2);
        BookManager.Instance.SetBookUnlocked("Thor3", 2);
        BookManager.Instance.SetBookUnlocked("Thor4", 2);
        BookManager.Instance.SetBookUnlocked("Surtr1", 2);
        BookManager.Instance.SetBookUnlocked("Surtr2", 2);
        BookManager.Instance.SetBookUnlocked("Surtr3", 2);
        BookManager.Instance.SetBookUnlocked("Surtr4", 2);

        if (BookManager.Instance.Blood == 0)
        {
            Debug.Log("pk");
            Debug.Log(BookManager.Instance.CheckBookEquipped("Thor1"));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
