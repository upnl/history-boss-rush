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
    [SerializeField] private PlayerController playerController;
    private string shopSceneName = "StoreSceneNew";
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
        playerController.TakeAwayControl();
        Time.timeScale = 0f;
    }
    public void Win()
    {
        AudioManager.Instance.PlaySfx(2);
        winPanel.SetActive(true);
        win = true;
        playerController.TakeAwayControl();
        Time.timeScale = 0f;
    }

    public void Pause()
    {
        previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        previousPlayerCanDash = playerController.CanDash;
        playerController.TakeAwayControl();
        dashTemp = true;
        pausePanel.SetActive(true);
        
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = previousTimeScale;
        if (Time.timeScale == 0) Time.timeScale = 1f;
        playerController.ReturnControl(previousPlayerCanDash);
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
                Time.timeScale = 1f;
                BookManager.Instance.ResetBookEquipped();
                SceneLoader.Instance.LoadScene(shopSceneName);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Pause();
        }
    }
}
