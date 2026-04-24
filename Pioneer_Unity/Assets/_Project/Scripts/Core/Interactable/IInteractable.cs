// IInteractable.cs
// Written by:      Jake Morgan
// Last Updated:    22/04/2026

using UnityEngine;

namespace Pioneer.Core.Interactables
{
    public interface IInteractable
    {
        // Method to be called when the drone interacts with this object
        void Interact(Transform droneTransform);

        // Optional: Method to get the name of the interactable for UI feedback
        string GetInteractableName();
    }
}

// end of IInteractable.cs