using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene2 : MonoBehaviour
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

    private void OnMouseOver()
    {
        gameObject.GetComponent<SpriteRenderer>().material.color = new Color(169 / 255f, 169 / 255f, 169 / 255f, 255 / 255f);

        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    private void OnMouseExit()
    {
        gameObject.GetComponent<SpriteRenderer>().material.color = Color.white;
    }
}
