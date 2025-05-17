using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The target the camera will follow (e.g., the player)
    public float smoothSpeed = 0.125f; // Smoothness of the camera movement
    public Vector3 offset; // Offset of the camera from the target

    public float zoomSpeed = 4f; // Speed of zooming
    public float minZoom = 5f; // Minimum zoom distance
    public float maxZoom = 20f; // Maximum zoom distance

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError("CameraFollow script must be attached to a Camera!");
        }
        //make cursor active
              Cursor.visible = true;
    }

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogError("Target is not assigned in CameraFollow script!");
            return;
        }

        // Smoothly follow the target
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Handle zooming with the mouse scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - scroll * zoomSpeed, minZoom, maxZoom);        }
    }
}