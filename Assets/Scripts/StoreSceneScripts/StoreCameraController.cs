using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreCameraController : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject player;

    private void Update() {
        mainCamera.transform.position = player.transform.position + new Vector3(0f, 0f, -10);
    }
}
