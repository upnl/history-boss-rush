using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene2 : MonoBehaviour
{
    public string sceneName = "";
    [TextArea] public string warningText = "";
    public string bossName = "";

    [SerializeField] public GameObject name;

    // Start is called before the first frame update
    void Start()
    {
        name.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        gameObject.GetComponent<SpriteRenderer>().material.color = new Color(169 / 255f, 169 / 255f, 169 / 255f, 255 / 255f);
        name.gameObject.SetActive(true);

        if (BookManager.Instance.CheckBossDefeated(bossName) == 1 && Input.GetMouseButtonDown(0))
        {
            PopUIManager.Instance.ShowPopUI(warningText);
            PopUIManager.Instance.nowObj = sceneName;
        }

        else if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    private void OnMouseExit()
    {
        gameObject.GetComponent<SpriteRenderer>().material.color = Color.white;
        name.gameObject.SetActive(false);
    }

}
