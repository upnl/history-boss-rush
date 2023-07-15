using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private PlayerMove playerMove;
    private QuestManager questManager;

    private float writingTime = 1f;
    private float attackAfterDelay = 0.5f;
    private bool isAlive = true;
    private bool alreadyWriting = false;
    private bool alreadyAttack = false;

    private void Start()
    {
        playerMove = this.gameObject.GetComponent<PlayerMove>();
        questManager = GameManager.Instance.QuestManager;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && questManager.HasQuest && !alreadyWriting && !alreadyAttack)
        {
            StartCoroutine(WritingBook());
        }
        else if(Input.GetMouseButtonDown(0) && !alreadyAttack && !alreadyAttack && isAlive)
        {
            Attack();
        }
    }

    private void Attack()
    {
        alreadyAttack = true;
        //TODO
        StartCoroutine(AttackDelay());
    }

    private IEnumerator AttackDelay()
    {
        playerMove.CanMove = false;
        yield return new WaitForSeconds(attackAfterDelay);
        playerMove.CanMove = true;
        alreadyAttack = false;
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
