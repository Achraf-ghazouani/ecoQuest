using UnityEngine;
using TMPro;
using System.Collections;

public class SaveTheEnergy : MonoBehaviour
{
    public Light[] lights;
    public GameObject[] switches;
    public GameObject[] objectsToTurnOffWithLights;
    public TextMeshProUGUI objectiveText;
    public GameObject completePanel;
public GameObject vfxPrefab;         // Assign in Inspector
public AudioClip completeSound;      // Assign in Inspector
public AudioSource EndSound;      // Assign in Inspector

    public AudioSource audioSource; // 🔊 Add this
    public AudioClip lightOffSound; // 🔊 And this

    private int lightsTurnedOff = 0;
    private int totalLights;
    public GameObject badge;
    void Start()
    {
        totalLights = lights.Length;
        UpdateObjectiveUI();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                for (int i = 0; i < switches.Length; i++)
                {
                    if (hit.collider.gameObject == switches[i])
                    {
                        ToggleLight(i);
                        break;
                    }
                }
            }
        }
    }

    private void ToggleLight(int index)
    {
        if (index >= 0 && index < lights.Length && lights[index].enabled)
        {
            lights[index].enabled = false;
            lightsTurnedOff++;
            UpdateObjectiveUI();

            // Play sound 🔊
            if (audioSource != null && lightOffSound != null)
            {
                audioSource.PlayOneShot(lightOffSound);
            }

            if (objectsToTurnOffWithLights != null && index < objectsToTurnOffWithLights.Length)
            {
                objectsToTurnOffWithLights[index].SetActive(false);
            }

            Debug.Log($"Light {index} turned off");

            if (lightsTurnedOff >= totalLights)
            {
                OnAllLightsTurnedOff();
            }
        }
    }

private void OnAllLightsTurnedOff()
{
    // Spawn VFX
    if (vfxPrefab != null)
    {
        Instantiate(vfxPrefab, transform.position, Quaternion.identity);
    }

    // Play sound
    if (audioSource != null && completeSound != null)
    {
        audioSource.PlayOneShot(completeSound);
    }

    // Start coroutine for delay
    StartCoroutine(ShowLightCompletePanelAfterDelay());
}

private IEnumerator ShowLightCompletePanelAfterDelay()
{
    yield return new WaitForSeconds(3f); // Wait 3 seconds

    if (completePanel != null)
    {
        completePanel.SetActive(true);
    }

    if (badge != null)
    {
        badge.SetActive(true);
    }
}
    private void UpdateObjectiveUI()
    {
        if (objectiveText != null)
        {
            objectiveText.text = $"Lights Turned Off: {lightsTurnedOff}/{totalLights}";
        }
    }
}
