using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHoverAppear : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        Debug.Log("hover");

        gameObject.GetComponent<Image>().color = new Color(255 / 255f, 137 / 255f, 137 / 255f, 255 / 255f);
    }

    private void OnMouseExit()
    {
        gameObject.GetComponent<Image>().color = Color.white;
    }
}
