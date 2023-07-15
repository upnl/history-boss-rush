using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public string bookDescription;
    [SerializeField] private Text _bloodAmount;
    [SerializeField] private Text _bookDescription;

    void Start()
    {
        bookDescription = "탐색할 역사책을 선택하자";
    }
    void Update()
    {
        _bloodAmount.text = Convert.ToString(BookManager.Instance.blood);
        _bookDescription.text = BookManager.Instance.bookDescription;
    }
}