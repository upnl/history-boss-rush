using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public string bookDescription;
    [SerializeField] private Text _bloodAmount;
    [SerializeField] private Text _bookDescription;

    [SerializeField] private GameObject _fightButton;

    [SerializeField] private GameObject _nextButton;
    [SerializeField] private Text _speaker;
    [SerializeField] private Text _dialogue;
    [SerializeField] private GameObject _textBackground;

    private int dialogueIndex = 0;

    void Awake()
    {
        bookDescription = "탐색할 역사책을 선택하자";
        _fightButton.SetActive(true);
        if (BookManager.Instance.dialogueDB.GetData().Count == 0)
        {
            _speaker.text = "";
            _dialogue.text = "";
            Destroy(_textBackground);
            Destroy(_nextButton);
        }
        while (dialogueIndex < BookManager.Instance.dialogueDB.GetData().Count && Convert.ToInt32(BookManager.Instance.dialogueDB.GetData()[dialogueIndex][0]) != BookManager.Instance.iteration)
        {
            dialogueIndex += 1;
        }
        if (dialogueIndex == BookManager.Instance.dialogueDB.GetData().Count)
        {
            _speaker.text = "";
            _dialogue.text = "";
            Destroy(_textBackground);
            Destroy(_nextButton);
        }
        _speaker.text = BookManager.Instance.dialogueDB.GetData()[dialogueIndex][1];
        _dialogue.text = BookManager.Instance.dialogueDB.GetData()[dialogueIndex][2];
    }
    void Update()
    {
        _bloodAmount.text = Convert.ToString(BookManager.Instance.blood);
        _bookDescription.text = BookManager.Instance.bookDescription;
    }

    public void OnClickFightButton()
    {
        Debug.Log("Move To Fight Scene");
        BookManager.Instance.ResetBlood();
        if (BookManager.Instance.thorDefeated)
        {
            SceneManager.LoadScene("");
        }
        else if (BookManager.Instance.surtrDefeated)
        {
            SceneManager.LoadScene("");
        }
    }

    public void OnClickNextButton()
    {
        dialogueIndex += 1;
        if (dialogueIndex >= BookManager.Instance.dialogueDB.GetData().Count || Convert.ToInt32(BookManager.Instance.dialogueDB.GetData()[dialogueIndex][0]) != BookManager.Instance.iteration)
        {
            _speaker.text = "";
            _dialogue.text = "";
            Destroy(_textBackground);
            Destroy(_nextButton);
        }
        else {
            _speaker.text = BookManager.Instance.dialogueDB.GetData()[dialogueIndex][1];
            _dialogue.text = BookManager.Instance.dialogueDB.GetData()[dialogueIndex][2];
        }
    }
}