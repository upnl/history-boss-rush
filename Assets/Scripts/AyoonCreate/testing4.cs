using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testing4 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BookManager.Instance.BossDefeated("Thor");
        BookManager.Instance.BossDefeated("Hell");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
