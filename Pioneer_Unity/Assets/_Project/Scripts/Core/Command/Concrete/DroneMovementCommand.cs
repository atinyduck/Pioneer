// DroneMovementCommand.cs
// Written by:      Jake Morgan
// Last Updated:    24/04/2026

using System.Collections;
using UnityEngine;
using Pioneer.Commands;

namespace Pioneer.Commands.Concrete
{
    public class DroneMovementCommand : ICommand
    {
        // --- FIELDS ---
        private readonly Transform droneTransform;
        private readonly float distance;
        private readonly float speed;

        // --- CONSTRUCTOR ---
        public DroneMovementCommand(Transform droneTransform, float distance, float speed)
        {
            this.droneTransform = droneTransform;
            this.distance = distance;
            this.speed = speed <= 0f ? 1.0f : speed; // Ensure speed is not zero to avoid division by zero
        }

        // --- METHODS ---
        public IEnumerator Execute()
        {
            float duration = distance / speed;
            Vector3 startPosition = droneTransform.position;
            // Target position based on local forward (Z-axis in 3D)
            Vector3 targetPosition = startPosition + (droneTransform.forward * distance);

            Debug.Log($"[Command] Starting Movement: Moving from {startPosition} to {targetPosition}");

            // Check for blocking colliders before translating the drone manually.
            Vector3 rayOrigin = startPosition + (droneTransform.forward * 0.1f);
            if (Physics.Raycast(rayOrigin, droneTransform.forward, out RaycastHit hit, distance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                if (!hit.collider.transform.IsChildOf(droneTransform))
                {
                    Debug.LogWarning($"[Command] Movement blocked by {hit.collider.name}. Cannot move to {targetPosition}");

                    FeedbackManager feedbackManager = Object.FindFirstObjectByType<FeedbackManager>();
                    if (feedbackManager != null)
                    {
                        feedbackManager.ShowFailure();
                    }

                    CommandQueue queue = Object.FindFirstObjectByType<CommandQueue>();
                    if (queue != null)
                    {
                        queue.Stop();
                    }

                    yield break;
                }
            }

            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                droneTransform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Snap to exact target to prevent floating point errors over time
            droneTransform.position = targetPosition; 
            Debug.Log($"[Command] Finished Movement: Arrived at {droneTransform.position}");
        }

        public string GetCommandName()
        {
            return $"Move Drone forward by {distance} units at {speed} units/sec";
        }
    }
}

// end of DroneMovementCommand.cs