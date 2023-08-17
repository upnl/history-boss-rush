using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeeperInteraction : MonoBehaviour
{
    [SerializeField] public GameObject textBox;
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

        /*
        if (Input.GetMouseButtonDown(0))
        {
            textBox.gameObject.SetActive(true);
            textBox.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            textBox.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }
        */
    }

    private void OnMouseExit()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.white;
    }
}
