using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorAppear : MonoBehaviour
{
    [SerializeField] private Texture2D _cursorTx; 
    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(_cursorTx, Vector2.zero, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
