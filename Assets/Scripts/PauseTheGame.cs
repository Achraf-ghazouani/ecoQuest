using UnityEngine;
using UnityEngine.UI;

public class PauseTheGame : MonoBehaviour
{
    public GameObject pausePanel;  // Assign your pause menu panel in Inspector
    public Button continueButton;  // Assign your continue button in Inspector

    private bool isPaused = false;

    void Start()
    {
        // Ensure the game starts unpaused
        Time.timeScale = 1f;
        isPaused = false;
        pausePanel.SetActive(false);

        // Assign resume action to continue button
        if (continueButton != null)
            continueButton.onClick.AddListener(ResumeGame);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    
    public void PauseGame()
{
    Debug.Log("Game Paused - Showing Panel");
    Time.timeScale = 0f;
    isPaused = true;
    pausePanel.SetActive(true);
}
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pausePanel.SetActive(false);
    }
}
