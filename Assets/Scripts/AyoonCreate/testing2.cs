using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testing2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BookManager.Instance.SetBookEquipped("Thor1", 2, 0);
        BookManager.Instance.SetBookEquipped("Thor2", 2, 0);
        BookManager.Instance.SetBookEquipped("Thor3", 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
