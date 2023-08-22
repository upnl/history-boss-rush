using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeeperInteraction : MonoBehaviour
{
    [SerializeField] public GameObject textBox;

    [Header("Dialogs")]

    public string[] dialogs1;
    public string[] dialogs2;
    public string[] dialogs3;
    public string[] dialogs4;
    public string[] dialogs5;
    public string[] dialogs6;
    public string[] dialogs7;
    public string[] dialogs8;

    public TextMeshProUGUI textObj;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseOver()
    {
        Debug.Log("hover");

        gameObject.GetComponent<Renderer>().material.color = new Color(169 / 255f, 169 / 255f, 169 / 255f, 255 / 255f);

        if (Input.GetMouseButtonDown(0) && !TypingManager.Instance.isTyping)
        {
            textBox.gameObject.SetActive(true);

            Interaction(Random.Range(0, 8));
        }
    }

    private void OnMouseExit()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.white;
    }

    private void Interaction(int num)
    {
        switch (num)
        {
            case 0:
                TypingManager.Instance.Typing(dialogs1, textObj);
                break;
            case 1:
                TypingManager.Instance.Typing(dialogs2, textObj);
                break;
            case 2:
                TypingManager.Instance.Typing(dialogs3, textObj);
                break;
            case 3:
                TypingManager.Instance.Typing(dialogs4, textObj);
                break;
            case 4:
                TypingManager.Instance.Typing(dialogs5, textObj);
                break;
            case 5:
                TypingManager.Instance.Typing(dialogs6, textObj);
                break;
            case 6:
                TypingManager.Instance.Typing(dialogs7, textObj);
                break;
            case 7:
                TypingManager.Instance.Typing(dialogs8, textObj);
                break;
        }
    }
}
