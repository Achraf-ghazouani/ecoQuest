using UnityEngine;

public class CableRenderer : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public int segmentCount = 20;
    public float waveHeight = 0.5f;  // Height of the sine wave
    public int waveCount = 2;        // Number of wave loops along the cable

    private LineRenderer lr;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = segmentCount + 1;
    }

    void Update()
    {
        Vector3 p0 = startPoint.position;
        Vector3 p1 = endPoint.position;

        for (int i = 0; i <= segmentCount; i++)
        {
            float t = i / (float)segmentCount;
            Vector3 point = Vector3.Lerp(p0, p1, t);

            // Apply wave offset on X-axis
            float waveOffset = Mathf.Sin(t * waveCount * Mathf.PI * 2) * waveHeight;
            point.x += waveOffset;

            lr.SetPosition(i, point);
        }
    }
}
