using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public string bookDescription;
    [SerializeField] private Text _bloodAmount;
    [SerializeField] private Text _bookDescription;
    [SerializeField] private GameObject _fightButton;

    void Start()
    {
        bookDescription = "탐색할 역사책을 선택하자";
        _fightButton.SetActive(true);
    }
    void Update()
    {
        _bloodAmount.text = Convert.ToString(BookManager.Instance.blood);
        _bookDescription.text = BookManager.Instance.bookDescription;
    }

    public void OnClickFightButton()
    {
        Debug.Log("Move To Fight Scene");
        // Code for changing to Boss Scene
    }
}