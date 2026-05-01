// DroneInteractCommand.cs
// Written by:      Jake Morgan
// Last Updated:    24/04/2026

using System.Collections;
using UnityEngine;

namespace Pioneer.Commands.Concrete
{
    public class DroneInteractCommand : ICommand
    {
        public enum InteractionMode
        {
            Auto,
            Pickup,
            Drop
        }

        // --- FIELDS ---
        private readonly Transform droneTransform;
        private readonly Transform carryPoint;
        private readonly float interactDistance;
        private readonly InteractionMode interactionMode;

        // --- CONSTRUCTOR ---
        public DroneInteractCommand(Transform droneTransform, Transform carryPoint, float interactDistance = 1.5f)
            : this(droneTransform, carryPoint, InteractionMode.Auto, interactDistance)
        {
        }

        public DroneInteractCommand(Transform droneTransform, Transform carryPoint, InteractionMode interactionMode, float interactDistance = 1.5f)
        {
            this.droneTransform = droneTransform;
            this.carryPoint = carryPoint;
            this.interactDistance = interactDistance;
            this.interactionMode = interactionMode;
        }

        // --- METHODS ---
        public IEnumerator Execute()
        {
            Debug.Log("[Command] Starting Interaction...");

            bool isHoldingBox = carryPoint != null && carryPoint.childCount > 0;

            if (interactionMode == InteractionMode.Drop)
            {
                if (!isHoldingBox)
                {
                    Debug.LogWarning("[Interact] Drop requested, but the drone is not holding a box.");
                    yield return new WaitForSeconds(0.2f);
                    yield break;
                }

                Debug.Log("[Interact] Dropping held box.");
                Transform box = carryPoint.GetChild(0);
                box.SetParent(null);

                // Place it in front of the drone (using 'forward' for 3D grid)
                box.position = droneTransform.position + (droneTransform.forward * interactDistance);
                PickableBox boxComponent = box.GetComponent<PickableBox>();
                if (boxComponent != null)
                {
                    boxComponent.IsHeld = false;
                }

                Rigidbody boxRb = box.GetComponent<Rigidbody>();
                if (boxRb != null)
                {
                    boxRb.isKinematic = false;
                    boxRb.detectCollisions = true;
                }

                Debug.Log($"[Interact] Box safely dropped at {box.position}");
            }
            else if (interactionMode == InteractionMode.Pickup)
            {
                if (isHoldingBox)
                {
                    Debug.LogWarning("[Interact] Pickup requested, but the drone is already holding a box.");
                    yield return new WaitForSeconds(0.2f);
                    yield break;
                }

                yield return TryPickupBox();
            }
            else
            {
                // Auto mode keeps the old behavior used by the keyboard shortcut.
                if (isHoldingBox)
                {
                    Debug.Log("[Interact] Auto mode detected a held box. Dropping it.");
                    Transform box = carryPoint.GetChild(0);
                    box.SetParent(null);
                    box.position = droneTransform.position + (droneTransform.forward * interactDistance);

                    PickableBox boxComponent = box.GetComponent<PickableBox>();
                    if (boxComponent != null)
                    {
                        boxComponent.IsHeld = false;
                    }

                    Rigidbody boxRb = box.GetComponent<Rigidbody>();
                    if (boxRb != null)
                    {
                        boxRb.isKinematic = false;
                        boxRb.detectCollisions = true;
                    }

                    Debug.Log($"[Interact] Box safely dropped at {box.position}");
                }
                else
                {
                    yield return TryPickupBox();
                }
            }

            // Momentary pause so the pickup animation/action registers visually before driving away
            yield return new WaitForSeconds(0.2f);
        }

        private IEnumerator TryPickupBox()
        {
            Debug.Log($"[Interact] Drone is empty-handed. Looking {interactDistance} units forward...");

            // Draw a debug line in the Scene view so you can physically see the interaction reach
            Debug.DrawRay(droneTransform.position, droneTransform.forward * interactDistance, Color.red, 2f);

            // Raycast forward (in the Z direction local to the drone) to find a box
            Vector3 rayOrigin = droneTransform.position + (droneTransform.forward * 0.1f) + (Vector3.up * 0.1f);
            if (Physics.Raycast(rayOrigin, droneTransform.forward, out RaycastHit hit, interactDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                Debug.Log($"[Interact] Raycast Hit: {hit.collider.name}");

                PickableBox box = hit.collider.GetComponentInParent<PickableBox>();
                if (box != null && !box.IsHeld)
                {
                    box.IsHeld = true;
                    // Parent to carry point so it follows the drone automatically
                    box.transform.SetParent(carryPoint);
                    box.transform.localPosition = Vector3.zero;

                    // Slightly lift the box up so it isn't clipping through the floor while carried
                    // If it has a rigidbody, ensure it doesn't try to bump the drone heavily
                    Rigidbody boxRb = box.GetComponent<Rigidbody>();
                    if (boxRb != null)
                    {
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
            }

            yield break;
        }

        public string GetCommandName()
        {
            return interactionMode switch
            {
                InteractionMode.Pickup => "Interact (Pickup)",
                InteractionMode.Drop => "Interact (Drop)",
                _ => "Interact (Auto Pickup/Drop)"
            };
        }
    }
}

// end of file