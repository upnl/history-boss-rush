using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartButtonScript : MonoBehaviour
{
    public void OnMouseDown()
    {
        Debug.Log("Move To StoreScene");
        SceneManager.LoadScene("StoreScene");
    }
}
