using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverAppear : MonoBehaviour
{
    public static GameObject HoverNow = null;

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

        gameObject.GetComponent<Renderer>().material.color = new Color(169 / 255f, 169 / 255f, 169 / 255f, 255 / 255f);
    }

    private void OnMouseExit()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.white;
    }
}
