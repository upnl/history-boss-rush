using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PopUIManager : MonoBehaviour
{
    public static PopUIManager Instance { get; private set; }

    public TextMeshProUGUI textObj;

    public string nowObj = "";

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

        // Start is called before the first frame update
        void Start()
    {
        HidePopUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowPopUI(string text)
    {
        gameObject.SetActive(true);
        textObj.text = text;
    }

    public void HidePopUI()
    {
        gameObject.SetActive(false);
        nowObj = "";
        textObj.text = "";
    }

    public void Move()
    {
        SceneManager.LoadScene(nowObj);
    }
}
