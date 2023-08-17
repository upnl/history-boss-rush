using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
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
        gameObject.GetComponent<Image>().color = new Color(255 / 255f, 137 / 255f, 137 / 255f, 255 / 255f);

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("move");
            SceneManager.LoadScene("FieldScene");
        }
    }

    private void OnMouseExit()
    {
        gameObject.GetComponent<Image>().color = Color.white;
    }
}
