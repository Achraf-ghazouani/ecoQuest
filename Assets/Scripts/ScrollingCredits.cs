using UnityEngine;
using TMPro;  // For TextMeshPro functionality

public class ScrollingCredits : MonoBehaviour
{
    // Reference to the TMP text component that holds the credits
    public TextMeshProUGUI creditsText;

    // The speed at which the credits scroll
    public float scrollSpeed = 50f;

    // The position where the credits should stop (above the screen)
    private float endPositionY;

    void Start()
    {
        // Set the position where the credits should stop (above the screen)
        endPositionY = Screen.height + creditsText.rectTransform.rect.height;
    }

    void Update()
    {
        // Scroll the credits upwards by modifying the Y position of the RectTransform
        creditsText.rectTransform.position += Vector3.up * scrollSpeed * Time.deltaTime;

        // Once the credits go off-screen, reset them to start from the initial position
        if (creditsText.rectTransform.position.y > endPositionY)
        {
            creditsText.rectTransform.position = new Vector3(0, -creditsText.rectTransform.rect.height, 0);
        }
    }
}
