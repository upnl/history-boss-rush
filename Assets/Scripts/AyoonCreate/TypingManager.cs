using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypingManager : MonoBehaviour
{
    public static TypingManager Instance { get; private set; }

    [Header("Times for each character")]
    public float timeForCharacter; //0.08이 기본.

    [Header("Times for each character when speed up")]
    public float timeForCharacter_Fast; //0.03이 빠른 텍스트.

    float characterTime; // 실제 적용되는 문자열 속도.

    //임시 저장되는 대화 오브젝트와 대화내용.
    string[] dialogsSave;
    TextMeshProUGUI tmpSave;

    public static bool isDialogEnd;

    public bool isTyping = false;

    public bool isTypingEnd = false; //타이핑이 끝났는가?
    int dialogNumber = 0; //대화 문단 숫자.

    float timer; //내부적으로 돌아가는 시간 타이머

    [SerializeField] public GameObject textBox;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        timer = timeForCharacter;
        characterTime = timeForCharacter;
    }

    public void Typing(string[] dialogs, TextMeshProUGUI textObj)
    {
        isDialogEnd = false;
        dialogsSave = dialogs;
        tmpSave = textObj;
        if (dialogNumber < dialogs.Length)
        {
            char[] chars = dialogs[dialogNumber].ToCharArray(); //받아온 다이얼 로그를 char로 변환.
            StartCoroutine(Typer(chars, textObj)); //레퍼런스로 넘겨보는거 테스트 해보자.
        }
        else
        {
            //문장이 끝났으므로 다른 문장을 받을 준비... 다이얼로그 초기화, 다이얼로그 세이브와 티엠피 세이브 초기화
            tmpSave.text = "";
            isDialogEnd = true; // 호출자는 다이알로그 엔드를 보고 다음 동작을 진행해주면 됨.
            dialogsSave = null;
            tmpSave = null;
            dialogNumber = 0;
            textBox.gameObject.SetActive(false);
        }
    }

    public void GetInputDown()
    {
        //인풋이 들어왔을때 -> 텍스트가 진행중이면 빠르게 진행되고 텍스트가 마감되어있으면 다음 텍스트로 넘어감.
        //그리고 인풋이 캔슬되면 다시 문자열 속도를 정상화 시켜야함.
        if (dialogsSave != null)
        {
            if (isTypingEnd)
            {
                tmpSave.text = ""; //비어있는 문장 넘겨서 초기화. 
                isTyping = false;
                Typing(dialogsSave, tmpSave);
            }
            else
            {
                characterTime = timeForCharacter_Fast; //빠른 문장 넘김.
            }
        }
    }

    public void GetInputUp()
    {
        //인풋이 끝났을때.
        if (dialogsSave != null)
        {
            characterTime = timeForCharacter;
        }
    }

    IEnumerator Typer(char[] chars, TextMeshProUGUI textObj)
    {
        int currentChar = 0;
        int charLength = chars.Length;
        isTypingEnd = false;
        isTyping = true;

        while (currentChar < charLength)
        {
            if (timer >= 0)
            {
                yield return null;
                timer -= Time.deltaTime;
            }
            else
            {
                textObj.text += chars[currentChar].ToString();
                currentChar++;
                timer = characterTime; //타이머 초기화
            }
        }
        if (currentChar >= charLength)
        {
            isTypingEnd = true;
            dialogNumber++;
            yield break;
        }
    }
}