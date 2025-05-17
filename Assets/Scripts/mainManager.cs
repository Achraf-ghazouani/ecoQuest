using UnityEngine;
using UnityEngine.SceneManagement;  // For scene loading
using UnityEngine.UI; // For button functionality
using TMPro; // For TextMeshPro functionality
using System.Collections; // For coroutines

public class MainMenu : MonoBehaviour
{
    // Reference to the panels
    public GameObject settingsPanel;
    public GameObject creditsPanel;
    public GameObject loadingPanel;  // Reference to the loading panel

    public GameObject BG;
    public Slider progressBar;      // Reference to the progress bar (Slider)
    public TextMeshProUGUI percentageText;  // Reference to the TMP text that will show percentage

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; // Force cursor to be shown and unlocked
    }

    // Function to start the game
    public void StartGame()
    {
        // Activate the loading panel
        loadingPanel.SetActive(true);
        BG.SetActive(false);


        // Start the loading process (simulate the progress bar filling for 5 seconds)
        StartCoroutine(LoadGameWithProgress());
    }

    // Function to open the settings panel
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        BG.SetActive(false);
    }

    // Function to close the settings panel
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        BG.SetActive(true);
    }

    // Function to open the credits panel
    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
        BG.SetActive(false);

    }

    // Function to close the credits panel
    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
        BG.SetActive(true);
    }

    // Function to quit the game
    public void QuitGame()
    {
        // If running in the Unity editor
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Coroutine to simulate a loading process with a progress bar
    private IEnumerator LoadGameWithProgress()
    {
        float timeToLoad = 5f; // Total time for the fake loading (5 seconds)
        float currentTime = 0f;

        while (currentTime < timeToLoad)
        {
            currentTime += Time.deltaTime;
            float progress = currentTime / timeToLoad;  // Get the progress (0 to 1)

            progressBar.value = progress;  // Update the progress bar
            percentageText.text = Mathf.FloorToInt(progress * 100) + "%";  // Update the percentage text

            yield return null;  // Wait until the next frame
        }

        // After the loading is complete, load the game scene
        SceneManager.LoadScene(1);
    }

    public void Ai()
    {
    UnityEngine.SceneManagement.SceneManager.LoadScene(5); // Change 1 to your desired scene index

    }
}
