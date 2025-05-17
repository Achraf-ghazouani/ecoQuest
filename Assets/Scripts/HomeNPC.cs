using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeNPC : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0); // Loads scene at index 0 when F1 is pressed
        }
    }
}
