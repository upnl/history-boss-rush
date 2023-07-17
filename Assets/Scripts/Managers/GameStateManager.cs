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
    private string shopSceneName = "StoreScene";
    private bool lose = false;
    private bool win = false;
    private float previousTimeScale;
    [HideInInspector] public bool previousPlayerCanDash;
    [HideInInspector] public bool dashTemp = false;
    private void Start()
    {
        Time.timeScale = 1f;
    }
    public void Lose()
    {
        AudioManager.Instance.PlaySfx(1);
        losePanel.SetActive(true);
        lose = true;
        playerMove.CanDash = false;
        Time.timeScale = 0f;
    }
    public void Win()
    {
        AudioManager.Instance.PlaySfx(2);
        winPanel.SetActive(true);
        win = true;
        playerMove.CanDash = false;
        Time.timeScale = 0f;
    }

    public void Pause()
    {
        previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        previousPlayerCanDash = playerMove.CanDash;
        playerMove.CanDash = false;
        dashTemp = true;
        pausePanel.SetActive(true);
        
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = previousTimeScale;
        if (Time.timeScale == 0) Time.timeScale = 1f;
        playerMove.CanDash = previousPlayerCanDash;
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
