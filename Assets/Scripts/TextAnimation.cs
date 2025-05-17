using UnityEngine;
using TMPro;

public class TextAnimation : MonoBehaviour
{
    public TextMeshProUGUI uiText; // Reference to the TextMeshProUGUI component
    public string[] texts; // Array of texts to display
    public float typingSpeed = 0.05f; // Speed of typing animation
    public GameObject dialoguePanel; // Reference to the dialogue panel
    public GameObject player; // Reference to the player GameObject
    public string triggerTag = "Quest"; // Tag to detect objects that restart the component

    public AudioClip[] voiceClips; // Array of voice clips matching each text
    private AudioSource audioSource; // Audio source to play voice clips

    private int currentTextIndex = 0; // Index of the current text
    private bool isTyping = false; // Flag to check if typing is in progress
    private MonoBehaviour PlayerController; // Reference to the player's controller script

    void Start()
    {
        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (uiText != null && texts.Length > 0)
        {
            PlayerController = player.GetComponent<MonoBehaviour>();
            if (PlayerController != null)
            {
                PlayerController.enabled = false;
            }

            dialoguePanel.SetActive(true);
            StartCoroutine(TypeText());
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                audioSource.Stop(); // Stop voice playback
                uiText.text = texts[currentTextIndex]; // Show full text
                isTyping = false;
            }
            else
            {
                currentTextIndex++;

                if (currentTextIndex < texts.Length)
                {
                    StartCoroutine(TypeText());
                }
                else
                {
                    EndDialogue();
                }
            }
        }
    }

    private System.Collections.IEnumerator TypeText()
    {
        isTyping = true;
        uiText.text = "";

        // Play voice clip if available
        if (voiceClips.Length > currentTextIndex && voiceClips[currentTextIndex] != null)
        {
            audioSource.clip = voiceClips[currentTextIndex];
            audioSource.Play();
        }

        foreach (char letter in texts[currentTextIndex].ToCharArray())
        {
            uiText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);

        if (PlayerController != null)
        {
            PlayerController.enabled = true;
        }

        audioSource.Stop(); // Ensure voice is stopped at the end
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggerTag))
        {
            RestartTextAnimation();
        }
    }

    private void RestartTextAnimation()
    {
        StopAllCoroutines();
        audioSource.Stop();
        currentTextIndex = 0;
        isTyping = false;
        dialoguePanel.SetActive(true);
        StartCoroutine(TypeText());
    }
}
