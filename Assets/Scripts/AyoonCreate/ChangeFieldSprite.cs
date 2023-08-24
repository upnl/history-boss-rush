using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFieldSprite : MonoBehaviour
{
    public string bossName = "";

    [SerializeField] public Sprite changedSprite;

    // Start is called before the first frame update
    void Start()
    {
        if (BookManager.Instance.CheckBossDefeated(bossName) == 1)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = changedSprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
