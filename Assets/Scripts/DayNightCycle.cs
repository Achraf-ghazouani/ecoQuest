using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 10f; // Degrees per second

    private void Update()
    {
        // Rotate around X axis over time
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
    }
}
