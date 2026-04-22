// Interactable.cs
// Written by:      Jake Morgan
// Last Updated:    22/04/2026

using UnityEngine;
using UnityEngine.Events;

namespace Pioneer.Core.Interactables
{
    public class Interactable : MonoBehaviour, IInteractable
    {
        [Tooltip("The name displayed for this interactable object")]
        [SerializeField] private string interactableName = "Interactable Object";
        
        [Tooltip("Events to trigger when the drone interacts with this object")]
        public UnityEvent onInteract;

        public void Interact(Transform droneTransform)
        {
            Debug.Log($"[{gameObject.name}] was interacted with by {droneTransform.name}!");
            onInteract?.Invoke();
        }

        public string GetInteractableName()
        {
            return interactableName;
        }
    }
}
