using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;  // For handling UI pointer events

public class UIHoverAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float scaleAmount = 1.1f;  // Max scale
    public float animationSpeed = 1f; // Speed of animation

    private Vector3 originalScale;
    private bool isHovered = false;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Animate scaling only when the cursor is hovering over the object
        if (isHovered)
        {
            float scaleFactor = animationSpeed * Time.unscaledDeltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale * scaleAmount, scaleFactor);
        }
        else
        {
            float scaleFactor = animationSpeed * Time.unscaledDeltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, scaleFactor);
        }
    }

    // Called when the mouse enters the UI element
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    // Called when the mouse exits the UI element
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }
}
