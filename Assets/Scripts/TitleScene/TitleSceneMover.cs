using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleSceneMover : MonoBehaviour
{
    public GameObject Story;
    public GameObject Tutorial1;
    public GameObject Tutorial2;

    static public bool StoryEnd = false;

    void Update()
    {
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            if (Story.activeSelf == false)
            {
                Story.SetActive(true);
            }

            else if (Tutorial1.activeSelf == false)
            {
                if (StoryEnd == true)
                {
                    Tutorial1.SetActive(true);
                }
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
