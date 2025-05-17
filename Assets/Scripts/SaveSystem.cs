using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private string scoreKey = "PlayerScore"; // The key to store the score

    void Start()
    {
        // Load the score when the game starts
        LoadScore();
    }

    // Save the score to PlayerPrefs
    public void SaveScore(int score)
    {
        PlayerPrefs.SetInt(scoreKey, score); // Save the score with the key
        PlayerPrefs.Save(); // Make sure to save the changes
        Debug.Log("Score saved: " + score);
    }

    // Load the score from PlayerPrefs
    public int LoadScore()
    {
        if (PlayerPrefs.HasKey(scoreKey)) // Check if the score exists
        {
            int score = PlayerPrefs.GetInt(scoreKey);
            Debug.Log("Score loaded: " + score);
            return score; // Return the loaded score
        }
        else
        {
            Debug.LogWarning("No score found. Returning 0.");
            return 0; // Default score if not found
        }
    }

    // Reset the score (if needed)
    public void ResetScore()
    {
        PlayerPrefs.DeleteKey(scoreKey); // Delete the saved score
        Debug.Log("Score reset.");
    }
}
