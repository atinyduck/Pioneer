// DroneInteractCommand.cs
// Written by:      Jake Morgan
// Last Updated:    22/04/2026

using System.Collections;
using UnityEngine;
using Pioneer.Commands;
using Pioneer.Core.Interactables;

namespace Pioneer.Commands.Concrete
{
    public class DroneInteractCommand : ICommand
    {
        // --- FIELDS ---
        private readonly Transform droneTransform;
        private readonly float interactionDuration;
        private readonly float interactRadius;

        // --- CONSTRUCTOR ---
        public DroneInteractCommand(Transform droneTransform, float interactionDuration = 0.5f, float interactRadius = 2.5f)
        {
            this.droneTransform = droneTransform;
            this.interactionDuration = interactionDuration;
            this.interactRadius = interactRadius;
        }

        // --- METHODS ---
        public IEnumerator Execute()
        {
            bool interactionSuccessful = false;

            // Try to find an interactable object overlapping the drone's position
            Collider[] hitColliders = Physics.OverlapSphere(droneTransform.position, interactRadius);
            
            foreach (var hit in hitColliders)
            {
                IInteractable interactable = hit.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact(droneTransform);
                    interactionSuccessful = true;
                    break; // Only interact with one object at a time
                }
            }

            if (!interactionSuccessful)
            {
                Debug.Log("No interactable object found nearby.");
            }

            // Simulate an interaction by waiting for the specified duration
            yield return new WaitForSeconds(interactionDuration);
        }

        public string GetCommandName()
        {
            return $"Interact (duration {interactionDuration}s)";
        }
    }
}
