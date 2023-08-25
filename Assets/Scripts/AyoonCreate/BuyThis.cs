using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class BuyThis : MonoBehaviour
{
    public string bookName = "";
    public int bookLevel = 0;
    public int bookPrice = 0;
    public bool equipped = false;

    public string[] bookExplain;
    public TextMeshProUGUI textObj;

    [SerializeField] public GameObject textBox;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseEnter()
    {
        gameObject.GetComponent<Renderer>().material.color = new Color(169 / 255f, 169 / 255f, 169 / 255f, 255 / 255f);

        TypingManager.Instance.InitiateText(textObj);
        textBox.gameObject.SetActive(true);
        TypingManager.Instance.Typing(bookExplain, textObj);
    }

    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && equipped == false)
        {
            if (bookPrice <= BookManager.Instance.Blood)
            {
                BookManager.Instance.SetBookEquipped(bookName, bookLevel, bookPrice);
                BookUIManager.Instance.ShowBookUI();
                equipped = true;
            }
        }
    }

    private void OnMouseExit()
    {
        if (equipped == true)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(169 / 255f, 169 / 255f, 169 / 255f, 255 / 255f);
        }

        else
        {
            gameObject.GetComponent<Renderer>().material.color = Color.white;
        }
    }
}
