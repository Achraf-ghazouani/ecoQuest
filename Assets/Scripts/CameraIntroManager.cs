using UnityEngine;

public class CameraIntroManager : MonoBehaviour
{
    public Animator cameraAnimator; // Reference to the Animator component for the intro camera
    public Camera introCamera; // Reference to the intro camera
    public Camera mainCamera; // Reference to the main camera
    public GameObject excludedCanvas; // Reference to the canvas or UI object to exclude
public GameObject canvasToDisable; // Assign this in the Inspector

    void Start()
    {
        excludedCanvas.SetActive(false); // Hide the excluded canvas at the start

        // Ensure the intro camera has higher depth at the start
        if (introCamera != null)
        {
            introCamera.depth = 1; // Higher depth means it renders on top

            // Exclude the canvas layer from the intro camera's culling mask
            if (excludedCanvas != null)
            {
                int excludedLayer = excludedCanvas.layer;
                introCamera.cullingMask &= ~(1 << excludedLayer);
            }
        }

        if (mainCamera != null)
        {
            mainCamera.depth = 0; // Lower depth means it renders behind
        }

        // Play the intro animation
        if (cameraAnimator != null)
        {
            cameraAnimator.Play("IntroAnimation"); // Replace with your animation clip name
        }
    }

    // This function is called when the animation ends
   public void OnIntroAnimationEnd()
{
    Debug.Log("Intro animation finished. Switching to the main camera...");

    // Disable the intro camera GameObject
    if (introCamera != null)
    {
        introCamera.gameObject.SetActive(false); // Disable the intro camera
        excludedCanvas.SetActive(true); // Show the previously excluded canvas
    }

    // Ensure the main camera is rendering
    if (mainCamera != null)
    {
        mainCamera.depth = 1; // Move main camera to the front
    }

    // Disable the other canvas (e.g., skip UI, logo, etc.)
    if (canvasToDisable != null)
    {
        canvasToDisable.SetActive(false);
    }
}
    public void SkipIntro()
{
    if (cameraAnimator != null)
    {
        cameraAnimator.enabled = false; // Stop the intro animation
    }

    OnIntroAnimationEnd(); // Immediately switch to the main camera
}
}