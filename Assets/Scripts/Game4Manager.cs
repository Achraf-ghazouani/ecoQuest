using UnityEngine;
public class Game4Manager : MonoBehaviour
{
    public Transform grabPoint;
    public float grabRange = 3f;
    public LayerMask cubeLayer;
    public LayerMask placeholderLayer;

    public AudioSource audioSource;
    public AudioClip placeSound;

    private GameObject grabbedObject = null;
    private Rigidbody grabbedRb = null;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (grabbedObject == null)
                TryGrabCube();
            else
                TryPlaceOnPlaceholder();
        }

        if (grabbedObject != null)
        {
            MoveGrabbedCube();
        }
    }

    private void TryGrabCube()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, grabRange, cubeLayer))
        {
            grabbedObject = hit.collider.gameObject;
            grabbedRb = grabbedObject.GetComponent<Rigidbody>();

            if (grabbedRb != null)
                grabbedRb.isKinematic = true;
        }
    }

    private void TryPlaceOnPlaceholder()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, grabRange, placeholderLayer))
        {
            CubePlaceholder placeholder = hit.collider.GetComponent<CubePlaceholder>();
            CubeItem cubeItem = grabbedObject.GetComponent<CubeItem>();

            if (placeholder != null && cubeItem != null && !placeholder.isOccupied)
            {
                if (placeholder.expectedCategory == cubeItem.category)
                {
                    grabbedObject.transform.position = placeholder.transform.position;
                    grabbedObject.GetComponent<Collider>().enabled = false;

                    placeholder.isOccupied = true;
                    grabbedRb = null;
                    grabbedObject = null;

                    UIManager.instance.AddScore(cubeItem.category);

                    if (audioSource != null && placeSound != null)
                    {
                        audioSource.PlayOneShot(placeSound);
                    }

                    Debug.Log("✅ Correct cube placed!");
                }
                else
                {
                    Debug.Log("❌ Incorrect placement. Try again.");
                }
            }
            else
            {
                Debug.Log("Can't place here. Either already occupied or invalid.");
            }
        }
        else
        {
            Debug.Log("No placeholder under mouse.");
        }
    }

    private void MoveGrabbedCube()
    {
        grabbedObject.transform.position = Vector3.Lerp(
            grabbedObject.transform.position,
            grabPoint.position,
            Time.deltaTime * 10f
        );
    }
}
