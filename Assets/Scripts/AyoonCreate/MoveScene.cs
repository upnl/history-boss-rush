using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    public string sceneName = "";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Enter()
    {
        gameObject.GetComponent<Image>().color = new Color(255 / 255f, 137 / 255f, 137 / 255f, 255 / 255f);
    }

    public void Exit()
    {
        gameObject.GetComponent<Image>().color = Color.white;
    }

    public void Click()
    {
        SceneLoader.Instance.LoadScene(sceneName);
    }
}
