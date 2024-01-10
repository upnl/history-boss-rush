using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodTextUIManager : MonoBehaviour
{
    public Text bloodText;

    private string _bloodStr;

    // Start is called before the first frame update
    void Start()
    {
        _bloodStr = BookManager.Instance.Blood.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        _bloodStr = BookManager.Instance.Blood.ToString();

        if (BookManager.Instance.Blood == BookManager.Instance.maxBlood)
        {
            bloodText.text = "<color=#FF8989>" + _bloodStr + "</color>";
        }

        else
        {
            bloodText.text = _bloodStr;
        } 
        
    }
}
