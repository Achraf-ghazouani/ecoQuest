using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string persistentSceneName; // Name of the scene to keep loaded

    void Start()
    {
        // Ensure the persistent scene is loaded
        if (!string.IsNullOrEmpty(persistentSceneName) && !SceneManager.GetSceneByName(persistentSceneName).isLoaded)
        {
            SceneManager.LoadScene(persistentSceneName, LoadSceneMode.Additive);
            Debug.Log($"Persistent scene '{persistentSceneName}' loaded.");
        }
    }

    public void SwitchToScene(string newSceneName)
    {
        // Load the new scene without unloading the persistent scene
        SceneManager.LoadScene(newSceneName, LoadSceneMode.Single);
        Debug.Log($"Switched to scene '{newSceneName}' while keeping '{persistentSceneName}' loaded.");
    }
}