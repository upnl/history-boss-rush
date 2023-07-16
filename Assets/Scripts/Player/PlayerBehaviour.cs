using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    private PlayerMove playerMove;
    private QuestManager questManager;
    [SerializeField] private GameObject slashPrefab;

    private float writingTime = 1f;
    private float attackAfterDelay = 0.5f;
    private bool isAlive = true;
    private bool alreadyWriting = false;
    private bool alreadyAttack = false;
    
    [SerializeField] private Slider writingSlider;

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
        else if(Input.GetMouseButtonDown(0) && !alreadyAttack && !alreadyWriting && isAlive)
        {
            Attack(Input.mousePosition);
        }
    }

    private void Attack(Vector3 mousePosition)
    {
        Vector3 mousePos = mousePosition;
        mousePos.z = 0;
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePos);
        alreadyAttack = true;
        float z = Quaternion.FromToRotation(Vector3.up, (targetPos - transform.position).normalized).eulerAngles.z;
        AudioManager.Instance.PlaySfx(0);
        GameObject slash = Instantiate<GameObject>(slashPrefab, transform.position, Quaternion.identity);
        slash.transform.localRotation = Quaternion.Euler(slash.transform.localRotation.x, slash.transform.localRotation.y, z + 90f);
        StartCoroutine(AttackDelay());
        Destroy(slash, 0.4f);
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
        writingSlider.gameObject.SetActive(true);
        writingSlider.value = 1f;
        alreadyWriting = true;
        playerMove.CanMove = false;
        GameManager.Instance.QuestManager.StopTimer = true;
        float elapsedTime = 0f;
        while(elapsedTime < writingTime)
        {
            elapsedTime += Time.deltaTime;
            writingSlider.value = 1f - elapsedTime/writingTime;
            yield return null;
        }
        if(isAlive)
        {
            playerMove.CanMove = true;
            GameManager.Instance.QuestManager.UnlockBook = true;
        }
        writingSlider.gameObject.SetActive(false);
        alreadyWriting = false;
    }
}
