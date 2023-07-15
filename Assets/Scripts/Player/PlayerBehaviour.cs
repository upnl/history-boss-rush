using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private PlayerMove playerMove;
    private QuestManager questManager;

    private float writingTime = 1f;
    private bool isAlive = true;
    private bool alreadyWriting = false;

    private void Start()
    {
        playerMove = this.gameObject.GetComponent<PlayerMove>();
        questManager = GameManager.Instance.QuestManager;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && questManager.HasQuest && !alreadyWriting)
        {
            StartCoroutine(WritingBook());
        }
    }

    private void Attack()
    {
        //TODO
    }

    public void GetDamaged()
    {
        isAlive = false;
        playerMove.CanMove = false;
        GameManager.Instance.GameStateManager.Lose();
    }

    private IEnumerator WritingBook()
    {
        alreadyWriting = true;
        playerMove.CanMove = false;
        GameManager.Instance.QuestManager.StopTimer = true;
        yield return new WaitForSeconds(writingTime);
        if(isAlive)
        {
            playerMove.CanMove = true;
            GameManager.Instance.QuestManager.UnlockBook = true;
        }
        alreadyWriting = false;
    }
}
