using UnityEngine;
using UnityEngine.AI;

public class StartLocationManager : MonoBehaviour
{
    public Transform player;
    public Transform startLocation;
    public float teleportDuration = 2f;
    public float flyHeight = 3f;

    private bool isTeleporting = false;
    public GameObject BackPnale;
 
    public GameObject[] panelsToClose; // 👈 Assign your 3 panels here in the Inspector

    public void TeleportToStartLocation()
    {
        if (player != null && startLocation != null)
        {
            StartCoroutine(FlyToStartLocation());
        }

        // 👇 Close any active panel
        foreach (GameObject panel in panelsToClose)
        {
            if (panel != null && panel.activeSelf)
            {
                panel.SetActive(false);
            }
        }
                BackPnale.SetActive(false);
 
    }

    private System.Collections.IEnumerator FlyToStartLocation()
    {
        if (isTeleporting) yield break;

        isTeleporting = true;

        NavMeshAgent agent = player.GetComponent<NavMeshAgent>();
        if (agent != null) agent.enabled = false;

        Vector3 start = player.position;
        Vector3 end = startLocation.position;
        Quaternion startRot = player.rotation;
        Quaternion endRot = startLocation.rotation;

        float elapsed = 0f;

        while (elapsed < teleportDuration)
        {
            float t = elapsed / teleportDuration;
            Vector3 midPoint = Vector3.Lerp(start, end, t);
            midPoint.y += Mathf.Sin(t * Mathf.PI) * flyHeight;
            player.position = midPoint;
            player.rotation = Quaternion.Slerp(startRot, endRot, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        player.position = end;
        player.rotation = endRot;

        if (agent != null)
        {
            agent.enabled = true;
            agent.ResetPath();
        }

        isTeleporting = false;
    }
}
