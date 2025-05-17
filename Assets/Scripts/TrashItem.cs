using UnityEngine;
public enum TrashCategory
{
    Plastic,
    Glass,
    Paper,
    Organic
}
public class TrashItem : MonoBehaviour
{
    public TrashCategory category; // Assign the category in the Inspector
}