using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private Collider doorCollider;
    [SerializeField] private MeshRenderer doorRenderer;

    [Header("Visual Colors")]
    [SerializeField] private Color lockedColor = Color.red;
    [SerializeField] private Color unlockedColor = Color.green;

    private void Start()
    {
        // Ensure defaults are grabbed if missing
        if (doorCollider == null) doorCollider = GetComponent<Collider>();
        if (doorRenderer == null) doorRenderer = GetComponent<MeshRenderer>();

        Close(); // Default to closed on startup
    }

    public void Open()
    {
        if (doorCollider != null) doorCollider.enabled = false;

        if (doorRenderer != null)
        {
            // Simple visual indication. 
            doorRenderer.material.color = unlockedColor; 
        }

        Debug.Log("Door Opened!");
    }

    public void Close()
    {
        if (doorCollider != null) doorCollider.enabled = true;

        if (doorRenderer != null)
        {
            doorRenderer.material.color = lockedColor;
        }

        Debug.Log("Door Closed!");
    }
}