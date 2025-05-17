using UnityEngine;

public class TurbinAnimation : MonoBehaviour
{
    public float rotationSpeed = 100f; // Speed of rotation in degrees per second

    // Update is called once per frame
    void Update()
    {
        // Rotate the object around the Z-axis
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}