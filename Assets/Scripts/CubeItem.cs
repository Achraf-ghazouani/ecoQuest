using UnityEngine;

public enum CubeCategory
{
    Toxic,
    Safe
}

public class CubeItem : MonoBehaviour
{
    public CubeCategory category;
}
