using UnityEngine;

public class LightproofBox : MonoBehaviour
{
    public Color boxColor = Color.black; // Color of the box
    public GameObject[] objectsInside; // Objects inside the box to disable lighting

    void Start()
    {
        // Create a new material with an Unlit Shader for the box
        Material unlitMaterial = new Material(Shader.Find("Unlit/Color"));
        unlitMaterial.color = boxColor;

        // Assign the material to the box's Renderer
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = unlitMaterial;
        }

        // Disable lighting for objects inside the box
        foreach (GameObject obj in objectsInside)
        {
            Renderer objRenderer = obj.GetComponent<Renderer>();
            if (objRenderer != null)
            {
                objRenderer.material.shader = Shader.Find("Unlit/Color");
            }
        }
    }
}