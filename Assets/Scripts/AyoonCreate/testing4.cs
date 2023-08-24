using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testing4 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!BookManager.Instance.thorDefeated)
        {
            Debug.Log("1");
        }

        BookManager.Instance.BossDefeated("Thor");

        if (BookManager.Instance.thorDefeated)
        {
            Debug.Log("2");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
