using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    public List<GameObject> dialoguePanels;
    public List<string> questTags;
    public GameObject quizPanel;
    public QuestStat questStat;

    public Transform player;
    public Transform teleportLocation;
    // Add two new teleport locations
    public Transform newTeleportLocation1;
    public Transform newTeleportLocation2;

    private GameObject activeDialoguePanel = null;
    private bool waitingForQuiz = false;
    private int activeQuestIndex = -1;

    public float flyDuration = 1.5f;
    public float flyHeight = 2f;
    public GameObject TrashCollect;
    private HashSet<string> completedTags = new HashSet<string>();

    public GameObject scorePanelQuest3;
    public GameObject scorePanelQuest4;

    // Reference to the PlayerController to disable it when the dialogue is active
    public MonoBehaviour playerController; // Assign the PlayerController script in Inspector
[Header("Background Music")]
public AudioSource bgAudioSource;
public AudioClip bgMusicDefault;
public AudioClip bgMusicQuest3;
public AudioClip bgMusicQuest4;
    void Start()
    {
        foreach (GameObject panel in dialoguePanels)
        {
            if (panel != null)
                panel.SetActive(false);
        }

        if (quizPanel != null)
            quizPanel.SetActive(false);
    }

    void Update()
    {
        // If the dialogue is closed and we are waiting for teleportation, perform the next action
        if (waitingForQuiz && activeDialoguePanel != null && !activeDialoguePanel.activeSelf)
        {
            waitingForQuiz = false;

            // Re-enable player movement once the dialogue panel is hidden
            if (playerController != null)
                playerController.enabled = true; // Re-enable movement

            // Handle quests based on active quest index
            if (activeQuestIndex == 0)
            {
                OpenQuizPanel();
            }
            else if (activeQuestIndex == 1)
            {
                StartCoroutine(FlyToNewPosition(flyDuration));
                TrashCollect.SetActive(true);
            }
            else if (activeQuestIndex == 2)
            {
                StartCoroutine(FlyToNewPositionForNewLocation(newTeleportLocation1, flyDuration));
            }
            else if (activeQuestIndex == 3)
            {
                StartCoroutine(FlyToNewPositionForNewLocation(newTeleportLocation2, flyDuration));
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (questTags.Count != dialoguePanels.Count)
        {
            Debug.LogError("Mismatch between questTags and dialoguePanels. Ensure they are aligned.");
            return;
        }

        for (int i = 0; i < questTags.Count; i++)
        {
            if (other.CompareTag(questTags[i]) && !completedTags.Contains(questTags[i]))
            {
                completedTags.Add(questTags[i]);

                if (dialoguePanels[i] == null)
                {
                    Debug.LogWarning($"No dialogue panel assigned for quest index {i}");
                    return;
                }

                // Set active dialogue panel
                activeDialoguePanel = dialoguePanels[i];
                activeDialoguePanel.SetActive(true);
                activeQuestIndex = i;

                // Disable player movement when dialogue is active
                if (playerController != null)
                    playerController.enabled = false; // Disable movement

                // Set the current quest state
                if (questStat != null)
                    questStat.SetCurrentQuest(i);

                // Enable teleportation for all quests
                waitingForQuiz = true;

                Debug.Log($"Quest {i} triggered. Dialogue panel activated.");
                break;
            }
        }
    }

    private void OpenQuizPanel()
    {
        if (quizPanel != null)
        {
            quizPanel.SetActive(true);

            if (questStat != null)
            {
                questStat.SetCurrentQuest(0);
                Debug.Log("Quest state set to quiz (0)");
            }
            else
            {
                Debug.LogWarning("QuestStat reference is not assigned!");
            }
        }
        else
        {
            Debug.LogWarning("Quiz panel is not assigned!");
        }
    }

    private bool isTeleporting = false;

    // Method for teleporting the player to the original teleport location
    public IEnumerator FlyToNewPosition(float duration)
    {
        if (player == null || teleportLocation == null)
        {
            Debug.LogWarning("Player or teleport location is not assigned!");
            yield break;
        }

        if (isTeleporting)
        {
            Debug.LogWarning("Teleportation already in progress!");
            yield break;
        }

        isTeleporting = true;

        UnityEngine.AI.NavMeshAgent agent = player.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = false; // Disable NavMeshAgent during teleportation
        }

        Vector3 start = player.position;
        Vector3 end = teleportLocation.position;
        Quaternion startRot = player.rotation;
        Quaternion endRot = teleportLocation.rotation;

        float elapsed = 0f;

        Debug.Log($"Starting teleportation. Initial position: {player.position}");

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            Vector3 midPoint = Vector3.Lerp(start, end, t);
            midPoint.y += Mathf.Sin(t * Mathf.PI) * flyHeight;

            player.position = midPoint;
            player.rotation = Quaternion.Slerp(startRot, endRot, t);

            Debug.Log($"Teleporting... Current position: {player.position}");

            elapsed += Time.deltaTime;
            yield return null;
        }

        player.position = end;
        player.rotation = endRot;

        if (agent != null)
        {
            agent.enabled = true; // Re-enable NavMeshAgent after teleportation
            agent.ResetPath();    // Clear the agent's path
        }

        isTeleporting = false;

        Debug.Log($"Teleportation complete. Final position: {player.position}");
         if (bgAudioSource != null && bgMusicDefault != null)
    {
        bgAudioSource.clip = bgMusicDefault;
        bgAudioSource.Play();
    }
    }








    // New method for teleporting to new locations after the dialogue ends
    public IEnumerator FlyToNewPositionForNewLocation(Transform newTeleportLocation, float duration)
    {
        if (player == null || newTeleportLocation == null)
        {
            Debug.LogWarning("Player or new teleport location is not assigned!");
            yield break;
        }

        if (isTeleporting)
        {
            Debug.LogWarning("Teleportation already in progress!");
            yield break;
        }

        isTeleporting = true;

        UnityEngine.AI.NavMeshAgent agent = player.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
            agent.enabled = false;

        Vector3 start = player.position;
        Vector3 end = newTeleportLocation.position;
        Quaternion startRot = player.rotation;
        Quaternion endRot = newTeleportLocation.rotation;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            Vector3 midPoint = Vector3.Lerp(start, end, t);
            midPoint.y += Mathf.Sin(t * Mathf.PI) * flyHeight;
            player.position = midPoint;
            player.rotation = Quaternion.Slerp(startRot, endRot, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        player.position = end;
        player.rotation = endRot;

        if (agent != null)
        {
            agent.enabled = true;
            agent.ResetPath();
        }

        isTeleporting = false;

        Debug.Log("Teleportation complete.");
 if (bgAudioSource != null)
    {
        if (newTeleportLocation == newTeleportLocation1 && bgMusicQuest3 != null)
        {
            bgAudioSource.clip = bgMusicQuest3;
            bgAudioSource.Play();
        }
        else if (newTeleportLocation == newTeleportLocation2 && bgMusicQuest4 != null)
        {
            bgAudioSource.clip = bgMusicQuest4;
            bgAudioSource.Play();
        }
    }
        // Activate score panels assigned from inspector
        if (newTeleportLocation == newTeleportLocation1 && scorePanelQuest3 != null)
        {
            scorePanelQuest3.SetActive(true);
        }
        else if (newTeleportLocation == newTeleportLocation2 && scorePanelQuest4 != null)
        {
            scorePanelQuest4.SetActive(true);
        }
    }
}
