using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    private bool isMuted = false; // Tracks whether the sound is muted
    public GameObject settingsPanel; // Reference to the settings panel
    public MonoBehaviour playerController; // Reference to the PlayerController component
    public Image soundToggleButtonImage; // Reference to the button's Image component
    public Sprite soundOnSprite; // Sprite for sound on
    public Sprite soundOffSprite; // Sprite for sound off

    // Function to load a scene by its index
    public void LoadSceneByIndex(int sceneIndex)
    {
        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            Debug.LogWarning("Invalid scene index: " + sceneIndex);
        }
    }

    // Function to mute or unmute the sound and update the button sprite
    public void ToggleSound()
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0 : 1; // Mute if true, restore if false

        // Update the button sprite
        if (soundToggleButtonImage != null)
        {
            soundToggleButtonImage.sprite = isMuted ? soundOffSprite : soundOnSprite;
        }

        Debug.Log("Sound " + (isMuted ? "Muted" : "Restored"));
    }

    public void SetLowQuality()
{
    Debug.Log("SetLowQuality function called");
    QualitySettings.SetQualityLevel(0); // Low quality
    QualitySettings.vSyncCount = 0; // Disable VSync
    QualitySettings.antiAliasing = 0; // Disable anti-aliasing
    QualitySettings.shadowDistance = 0; // Decrease shadow distance
    Debug.Log("Graphics set to Low");
}

public void SetMediumQuality()
{
    Debug.Log("SetMediumQuality function called");
    QualitySettings.SetQualityLevel(2); // Medium quality
    QualitySettings.vSyncCount = 1; // Enable VSync
    QualitySettings.antiAliasing = 2; // Enable anti-aliasing
    QualitySettings.shadowDistance = 20; // Increase shadow distance
    QualitySettings.shadowResolution = ShadowResolution.Medium; // Increase shadow resolution
    Debug.Log("Graphics set to Medium");
}

public void SetHighQuality()
{
    Debug.Log("SetHighQuality function called");
    QualitySettings.SetQualityLevel(5); // High quality
    QualitySettings.vSyncCount = 1; // Enable VSync
    QualitySettings.antiAliasing = 4; // Enable anti-aliasing
    QualitySettings.shadowDistance = 50; // Increase shadow distance
    QualitySettings.shadowResolution = ShadowResolution.High; // Increase shadow resolution
    Debug.Log("Graphics set to High");
}
    // Function to toggle the settings panel
    public void ToggleSettingsPanel()
    {
        if (settingsPanel != null)
        {
            bool isActive = settingsPanel.activeSelf;
            settingsPanel.SetActive(!isActive); // Toggle the panel's active state

            if (playerController != null)
            {
                playerController.enabled = isActive; // Disable when panel is active, enable when inactive
            }
        }
    }
}