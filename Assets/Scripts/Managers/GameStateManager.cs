using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private PlayerMove playerMove;
    private string shopSceneName;
    private bool lose = false;
    private bool win = false;
    private float previousTimeScale;
    [HideInInspector] public bool previousPlayerCanDash;
    [HideInInspector] public bool dashTemp = false;
    public void Lose()
    {
        losePanel.SetActive(true);
        lose = true;
        playerMove.canDash = false;
    }
    public void Win()
    {
        winPanel.SetActive(true);
        win = true;
        playerMove.canDash = false;
    }

    public void Pause()
    {
        previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        previousPlayerCanDash = playerMove.canDash;
        playerMove.canDash = false;
        dashTemp = true;
        pausePanel.SetActive(true);
        
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = previousTimeScale;
        playerMove.canDash = previousPlayerCanDash;
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void Update()
    {
        if (lose || win)
        {
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene(shopSceneName);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }
}
