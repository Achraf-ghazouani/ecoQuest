using TMPro;
using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public TextMeshProUGUI safeScoreText;
    public TextMeshProUGUI toxicScoreText;
    public GameObject completePanel; // ✅ Completion panel
public GameObject vfxPrefab;          // VFX prefab (assign in Inspector)
public AudioClip completeSound;       // Sound to play (assign in Inspector)
public AudioSource audioSource; 
    private int safeScore = 0;
    private int toxicScore = 0;

    public int totalSafeRequired = 5;  // You can adjust these as needed
    public int totalToxicRequired = 5;
    public GameObject badge;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        UpdateScoreUI();
    }

    public void AddScore(CubeCategory category)
    {
        if (category == CubeCategory.Safe)
        {
            safeScore++;
        }
        else if (category == CubeCategory.Toxic)
        {
            toxicScore++;
        }

        UpdateScoreUI();
        CheckIfObjectiveComplete();
    }

    private void UpdateScoreUI()
    {
        safeScoreText.text = "Safe Elements: " + safeScore;
        toxicScoreText.text = "Toxic Elements: " + toxicScore;
    }

  private void CheckIfObjectiveComplete()
{
    if (safeScore >= totalSafeRequired && toxicScore >= totalToxicRequired)
    {
        // ✅ Play sound
        if (audioSource != null && completeSound != null)
        {
            audioSource.PlayOneShot(completeSound);
        }

        // ✅ Spawn VFX
        if (vfxPrefab != null)
        {
            Instantiate(vfxPrefab, transform.position, Quaternion.identity);
        }

        // ✅ Delay panel showing
        StartCoroutine(ShowFinalUIAfterDelay());
    }
}

    
    private IEnumerator ShowFinalUIAfterDelay()
{
    yield return new WaitForSeconds(3f);

    if (completePanel != null)
        completePanel.SetActive(true);

    if (badge != null)
        badge.SetActive(true);
}
}
