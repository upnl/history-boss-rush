using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleSceneMover : MonoBehaviour
{
    public GameObject Tutorial1;
    public GameObject Tutorial2;

    void Update()
    {
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            if (Tutorial1.activeSelf == false)
            {
                Tutorial1.SetActive(true);
            }
            else
            {
                if (Tutorial2.activeSelf == false)
                {
                    Tutorial2.SetActive(true);
                }
                else
                {
                    SceneManager.LoadScene("StoreScene");
                }
            }
        }
    }
}
