using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class LoadSceneByIndex : MonoBehaviour
{
    // Method to load a scene by its index
    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}