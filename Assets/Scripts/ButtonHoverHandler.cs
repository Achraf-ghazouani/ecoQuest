using UnityEngine;
using UnityEngine.EventSystems; // Required for pointer events

public class ButtonHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject targetObject; // The GameObject to show/hide on hover

    // Called when the pointer enters the button
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (targetObject != null)
        {
            targetObject.SetActive(true); // Show the GameObject
        }
    }

    // Called when the pointer exits the button
    public void OnPointerExit(PointerEventData eventData)
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false); // Hide the GameObject
        }
    }
}