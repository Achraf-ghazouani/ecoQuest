using UnityEngine;

public class QuestStat : MonoBehaviour
{
    public int currentQuestIndex = -1; // Index of the current quest (-1 means no active quest)
    public string[] questNames; // Array of quest names for tracking

    // Method to set the current quest
    public void SetCurrentQuest(int questIndex)
    {
        if (questIndex >= 0 && questIndex < questNames.Length)
        {
            currentQuestIndex = questIndex;
            Debug.Log($"Current Quest: {questNames[currentQuestIndex]}");
        }
        else
        {
            Debug.LogWarning("Invalid quest index!");
        }
    }

    // Method to clear the current quest
    public void ClearCurrentQuest()
    {
        currentQuestIndex = -1;
        Debug.Log("No active quest.");
    }

    // Method to get the current quest name
    public string GetCurrentQuestName()
    {
        if (currentQuestIndex >= 0 && currentQuestIndex < questNames.Length)
        {
            return questNames[currentQuestIndex];
        }
        return "No active quest";
    }
}