// FloorButton.cs
// Written by:      Jake Morgan
// Last Updated:    21/04/2026

using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class FloorButton : MonoBehaviour
{
    // --- FIELDS ---
    [Tooltip("Is the button currently pressed down?")]
    public bool IsPressed { get; private set; }

    public UnityEvent OnPressed;
    public UnityEvent OnReleased;

    private int objectsOnButton = 0;

    // --- LIFECYCLE ---
    private void OnTriggerEnter(Collider other)
    {
        // Check if what stepped on us is the Drone (needs 'Player' tag) or a Box
        if (this.CheckTags(other))
        {
            objectsOnButton++;
            if (objectsOnButton == 1) 
            {
                IsPressed = true;
                OnPressed?.Invoke(); 
                Debug.Log($"Button {gameObject.name} Pressed!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (this.CheckTags(other))
        {
            objectsOnButton--;
            if (objectsOnButton <= 0) 
            {
                objectsOnButton = 0;
                IsPressed = false;
                OnReleased?.Invoke();
                Debug.Log($"Button {gameObject.name} Released!");
            }
        }
    }

    private bool CheckTags(Collider other)
    {
        return other.CompareTag("Player") || other.CompareTag("PickableBox");
    }
}