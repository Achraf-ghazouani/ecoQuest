using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;

    [Header("Settings")]
    public float rotationSpeed = 10f; // Smooth rotation speed
    public float speed = 3.5f;         // Movement speed adjustable from Inspector

    [Header("Placement Mark")]
    public GameObject placementMarkPrefab; // Assign your VFX prefab here
    public float placementMarkLifetime = 3f; // How long the mark stays before disappearing (optional)

    [Header("Sound")]
    public AudioClip markSpawnSound; // Assign the sound clip for the mark spawn
    private AudioSource audioSource; // The AudioSource to play sounds

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;

        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component

        if (Camera.main == null)
        {
            Debug.LogError("No camera tagged as 'MainCamera' found in the scene!");
        }

        if (placementMarkPrefab == null)
        {
            Debug.LogWarning("Placement mark prefab not assigned!");
        }

        if (audioSource == null)
        {
            Debug.LogWarning("No AudioSource component found! Please attach one to the player.");
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right mouse button
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                agent.SetDestination(hit.point);

                SpawnPlacementMark(hit.point);
            }
        }

        SmoothRotateTowardsMoveDirection();
    }

    private void SmoothRotateTowardsMoveDirection()
    {
        if (agent.velocity.sqrMagnitude > 0.1f) // Only rotate when moving
        {
            Quaternion targetRotation = Quaternion.LookRotation(agent.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void SpawnPlacementMark(Vector3 position)
    {
        if (placementMarkPrefab != null)
        {
            GameObject mark = Instantiate(placementMarkPrefab, position, placementMarkPrefab.transform.rotation);

            // Play the sound when the mark is spawned
            if (markSpawnSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(markSpawnSound);
            }

            if (placementMarkLifetime > 0f)
            {
                Destroy(mark, placementMarkLifetime);
            }
        }
    }

    private void OnValidate()
    {
        if (agent != null)
        {
            agent.speed = speed;
        }
    }
}
