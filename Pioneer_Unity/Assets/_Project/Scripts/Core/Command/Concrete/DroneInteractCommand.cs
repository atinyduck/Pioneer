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
<<<<<<< Updated upstream
                IInteractable interactable = hit.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact(droneTransform);
                    interactionSuccessful = true;
                    break; // Only interact with one object at a time
=======
                Debug.Log("[Interact] Drone is holding a box. Dropping it.");
                Transform box = carryPoint.GetChild(0);
                box.SetParent(null);
                
                // Place it in front of the drone (using 'forward' for 3D grid)
                box.position = droneTransform.position + droneTransform.forward;
                box.GetComponent<PickableBox>().IsHeld = false;
                Debug.Log($"[Interact] Box safely dropped at {box.position}");
            }
            else
            {
                Debug.Log($"[Interact] Drone is empty-handed. Looking {interactDistance} units forward...");
                
                // Draw a debug line in the Scene view so you can physically see the interaction reach
                Debug.DrawRay(droneTransform.position, droneTransform.forward * interactDistance, Color.red, 2f);

                // 2. We aren't holding a box, try to pick one up
                // Raycast forward (in the Z direction local to the drone) to find a box
                if (Physics.Raycast(droneTransform.position, droneTransform.forward, out RaycastHit hit, interactDistance))
                {
                    Debug.Log($"[Interact] Raycast Hit: {hit.collider.name}");

                    PickableBox box = hit.collider.GetComponent<PickableBox>();
                    if (box != null && !box.IsHeld)
                    {
                        box.IsHeld = true;
                        // Parent to carry point so it follows the drone automatically
                        hit.transform.SetParent(carryPoint);
                        hit.transform.localPosition = Vector3.zero;
                        // Slightly lift the box up so it isn't clipping through the floor while carried
                        // If it has a rigidbody, ensure it doesn't try to bump the drone heavily
                        Rigidbody boxRb = box.GetComponent<Rigidbody>();
                        if (boxRb != null) {
                             boxRb.isKinematic = true;
                        }

                        Debug.Log($"[Interact] Successfully Picked Up: {box.name}");
                    }
                    else
                    {
                        Debug.LogWarning("[Interact] Hit an object, but it wasn't a valid PickableBox.");
                    }
                }
                else
                {
                    Debug.Log("[Interact] Raycast hit absolutely nothing. Space is clear.");
>>>>>>> Stashed changes
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
