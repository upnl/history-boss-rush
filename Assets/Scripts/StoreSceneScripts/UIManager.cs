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
    [SerializeField] private GameObject _dialogueCurtain;
    [SerializeField] private GameObject _keeper;

    private int dialogueIndex = 0;
    public bool dialogueActive = true;

    void Awake()
    {
        _fightButton.SetActive(false);
        var curtainPosition = _dialogueCurtain.transform.position;
        curtainPosition.z = 0f;
        _dialogueCurtain.transform.position = curtainPosition;
        var keeperPosition = _keeper.transform.position;
        keeperPosition.z = -2f;
        _keeper.transform.position = keeperPosition;
        bookDescription = "";
        if (BookManager.Instance.dialogueDB.GetData().Count == 0)
        {
            _speaker.text = "";
            _dialogue.text = "";
            dialogueActive = false;
            _fightButton.SetActive(true);
            curtainPosition = _dialogueCurtain.transform.position;
            curtainPosition.z = -15f;
            _dialogueCurtain.transform.position = curtainPosition;
            keeperPosition = _keeper.transform.position;
            keeperPosition.z = 2f;
            _keeper.transform.position = keeperPosition;
            Destroy(_textBackground);
            Destroy(_nextButton);
        }
        while (dialogueIndex < BookManager.Instance.dialogueDB.GetData().Count && Convert.ToInt32(BookManager.Instance.dialogueDB.GetData()[dialogueIndex][0]) != BookManager.Instance.iteration)
        {
            dialogueIndex += 1;
        }
        if (dialogueIndex >= BookManager.Instance.dialogueDB.GetData().Count)
        {
            _speaker.text = "";
            _dialogue.text = "";
            dialogueActive = false;
            _fightButton.SetActive(true);
            curtainPosition = _dialogueCurtain.transform.position;
            curtainPosition.z = -15f;
            _dialogueCurtain.transform.position = curtainPosition;
            keeperPosition = _keeper.transform.position;
            keeperPosition.z = 2f;
            _keeper.transform.position = keeperPosition;
            Destroy(_textBackground);
            Destroy(_nextButton);
        } else {
        _speaker.text = BookManager.Instance.dialogueDB.GetData()[dialogueIndex][1];
        _dialogue.text = BookManager.Instance.dialogueDB.GetData()[dialogueIndex][2];
        }
    }
    void Update()
    {
        _bloodAmount.text = Convert.ToString(BookManager.Instance.blood);
        _bookDescription.text = BookManager.Instance.bookDescription;
    }

    public void OnClickFightButton()
    {
        if (!dialogueActive)
        {
            Debug.Log("Move To Fight Scene");
            BookManager.Instance.ResetBlood();
            BookManager.Instance.iteration += 1;
            if (!BookManager.Instance.thorDefeated)
            {
                SceneManager.LoadScene("Boss_Thor");
            }
            else if (!BookManager.Instance.surtrDefeated)
            {
                //SceneManager.LoadScene("");
            }
        }
    }

    public void OnClickNextButton()
    {
        dialogueIndex += 1;
        if (dialogueIndex >= BookManager.Instance.dialogueDB.GetData().Count || Convert.ToInt32(BookManager.Instance.dialogueDB.GetData()[dialogueIndex][0]) != BookManager.Instance.iteration)
        {
            _speaker.text = "";
            _dialogue.text = "";
            dialogueActive = false;
            _fightButton.SetActive(true);
            var curtainPosition = _dialogueCurtain.transform.position;
            curtainPosition.z = -15f;
            _dialogueCurtain.transform.position = curtainPosition;
            var keeperPosition = _keeper.transform.position;
            keeperPosition.z = 2f;
            _keeper.transform.position = keeperPosition;
            Destroy(_textBackground);
            Destroy(_nextButton);
        }
        else {
            _speaker.text = BookManager.Instance.dialogueDB.GetData()[dialogueIndex][1];
            _dialogue.text = BookManager.Instance.dialogueDB.GetData()[dialogueIndex][2];
        }
    }
}