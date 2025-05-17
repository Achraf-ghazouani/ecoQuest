using UnityEngine;
using TMPro;
using System.Collections;


public class TrashCollectManager : MonoBehaviour
{
    public Transform grabPoint; // The point where the object will be held
    public float grabRange = 2f; // Maximum distance to grab an object
    public LayerMask grabbableLayer; // Layer for grabbable objects
    public LayerMask binLayer; // Layer for bins

    public Transform plasticBin; // Assign the Plastic bin in the Inspector
    public Transform GlasslBin; // Assign the Glass bin in the Inspector
    public Transform paperBin; // Assign the Paper bin in the Inspector
    public Transform organicBin; // Assign the Organic bin in the Inspector

    public int trashToCollect = 10; // Number of trash items to collect before showing completion

    public TextMeshProUGUI trashCounterText; // Reference to the TMP text for the trash counter
    public GameObject SaveTheEnergy; // UI to show when done
    public GameObject TrashCollect; // UI to hide when done
    public GameObject completePanel; // ✅ Assign this in the Inspector to show when all trash is collected
    public GameObject vfxPrefab;         // Assign your VFX prefab in the Inspector
public AudioClip completeSound;      // Assign your sound clip
public AudioSource audioSource;      // Assign an AudioSource component

    private int collectedTrashCount = 0; // Tracks the number of collected trash items
    private GameObject grabbedObject = null; // The currently grabbed object
    private Rigidbody grabbedObjectRb = null; // Rigidbody of the grabbed object

    [Header("Sound")]
    public AudioClip correctBinSound; // Sound to play when trash is placed in the correct bin
    private AudioSource EndSound; // AudioSource to play sounds
    public GameObject badge;

    void Start()
    {
        UpdateTrashCounterUI();
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
        if (audioSource == null)
        {
            Debug.LogWarning("No AudioSource component found! Please attach one to the player.");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button to grab/release
        {
            if (grabbedObject == null)
            {
                TryGrabObject();
            }
            else
            {
                TryPlaceInBin();
            }
        }

        if (grabbedObject != null)
        {
            MoveGrabbedObject();
        }
    }

    private void TryGrabObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, grabRange, grabbableLayer))
        {
            grabbedObject = hit.collider.gameObject;
            grabbedObjectRb = grabbedObject.GetComponent<Rigidbody>();

            if (grabbedObjectRb != null)
            {
                grabbedObjectRb.isKinematic = true;
                Debug.Log($"Grabbed object: {grabbedObject.name}");
            }
        }
    }

    private void TryPlaceInBin()
    {
        if (grabbedObject == null)
        {
            Debug.LogWarning("No object is currently grabbed!");
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, grabRange, binLayer))
        {
            Transform clickedBin = hit.collider.transform;

            TrashItem trashItem = grabbedObject.GetComponent<TrashItem>();
            if (trashItem == null)
            {
                Debug.LogWarning("The grabbed object does not have a TrashItem component!");
                return;
            }

            if (IsCorrectBin(trashItem.category, clickedBin))
            {
                Vector3 randomOffset = new Vector3(
                    Random.Range(-0.4f, 0.4f),
                    0.1f,
                    Random.Range(-0.4f, 0.4f)
                );

                grabbedObject.transform.position = clickedBin.position + randomOffset;

                if (grabbedObjectRb != null)
                {
                    grabbedObjectRb.isKinematic = true;
                }

                Collider objectCollider = grabbedObject.GetComponent<Collider>();
                if (objectCollider != null)
                {
                    objectCollider.enabled = false;
                }

                Debug.Log($"Placed {grabbedObject.name} in correct bin: {trashItem.category}");

                // Play sound when trash is placed correctly
                if (correctBinSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(correctBinSound);
                }

                grabbedObject = null;
                grabbedObjectRb = null;

                collectedTrashCount++;
                Debug.Log($"Collected trash count: {collectedTrashCount}/{trashToCollect}");

                UpdateTrashCounterUI();

                if (collectedTrashCount >= trashToCollect)
                {
                    OnTrashCollectionComplete();
                }
            }
            else
            {
                Debug.LogWarning($"Incorrect bin for {grabbedObject.name}. Try again!");
            }
        }
        else
        {
            Debug.LogWarning("No bin detected under the mouse click.");
        }
    }

    private bool IsCorrectBin(TrashCategory category, Transform bin)
    {
        switch (category)
        {
            case TrashCategory.Plastic:
                return bin == plasticBin;
            case TrashCategory.Glass:
                return bin == GlasslBin;
            case TrashCategory.Paper:
                return bin == paperBin;
            case TrashCategory.Organic:
                return bin == organicBin;
            default:
                return false;
        }
    }

    private void MoveGrabbedObject()
    {
        grabbedObject.transform.position = Vector3.Lerp(
            grabbedObject.transform.position,
            grabPoint.position,
            Time.deltaTime * 10f
        );
    }

   private void OnTrashCollectionComplete()
{
    // Spawn VFX at desired position (e.g., center of screen or a specific object)
    if (vfxPrefab != null)
    {
        Instantiate(vfxPrefab, transform.position, Quaternion.identity);
    }

    // Play sound
    if (audioSource != null && completeSound != null)
    {
        audioSource.PlayOneShot(completeSound);
    }

    // Start the delay coroutine
    StartCoroutine(ShowCompletionAfterDelay());
    
    TrashCollect.SetActive(false); // Hide trash UI
    collectedTrashCount = 0;       // Reset count
    UpdateTrashCounterUI();
    badge.SetActive(true);         // Show badge
}

private IEnumerator ShowCompletionAfterDelay()
{
    yield return new WaitForSeconds(3f); // Wait 3 seconds

    if (completePanel != null)
    {
        completePanel.SetActive(true);
    }
}

    private void UpdateTrashCounterUI()
    {
        if (trashCounterText != null)
        {
            trashCounterText.text = $"Trash Collected: {collectedTrashCount}/{trashToCollect}";
        }
    }
}
