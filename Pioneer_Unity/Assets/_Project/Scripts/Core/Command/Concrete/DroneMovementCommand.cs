// DroneMovementCommand.cs
// Written by:      Jake Morgan
// Last Updated:    15/03/2026

using System.Collections;
using UnityEngine;
using Pioneer.Commands;
using Pioneer.Puzzle;

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
            this.speed = speed <= 0f ? speed : 1.0f; // Ensure speed is not zero to avoid division by zero
        }

        // --- METHODS ---
        public IEnumerator Execute()
        {
            Vector3 startPosition = droneTransform.position;
<<<<<<< Updated upstream

            // In 2D, transform.up acts as our "forward" direction.
            Vector3 targetPosition = startPosition + (droneTransform.up * distance);
=======
            Vector3 targetPosition = startPosition + (droneTransform.forward * distance);

            // --- COLLISION CHECK (WALLS / DOORS) ---
            // Raycast forward to see if the path is actually clear before we try to move.
            // We cast from slightly above the ground to avoid hitting floor tiles.
            Vector3 rayStart = startPosition + Vector3.up * 0.5f;

            // Draw a visible line in the Unity Scene View so you can see exactly where the drone is looking!
            Debug.DrawRay(rayStart, droneTransform.forward * (distance + 0.1f), Color.yellow, 2f);

            // We add a tiny bit of extra distance (0.1f) in case the wall isn't perfectly aligned
            if (Physics.Raycast(rayStart, droneTransform.forward, out RaycastHit hit, distance + 0.1f))
            {
                // Ignore the floor if the ray somehow angled downwards
                if (!hit.collider.gameObject.name.ToLower().Contains("floor"))
                {
                    PickableBox box = hit.collider.GetComponent<PickableBox>();
                    if (box == null) 
                    {
                        Debug.LogWarning($"[Command] Movement Blocked! Drone hit: {hit.collider.gameObject.name} at distance {hit.distance}");

                        UI.CodeEditorUI editorUI = Object.FindObjectOfType<UI.CodeEditorUI>();
                        if (editorUI != null)
                        {
                            editorUI.UpdateStatus($"<color=red>Crash! The Pioneer hit a {hit.collider.gameObject.name}.</color>");
                        }

                        // Trigger puzzle failed screen
                        UI.FeedbackManager feedbackManager = Object.FindObjectOfType<UI.FeedbackManager>();
                        if (feedbackManager != null)
                        {
                            feedbackManager.ShowFailure();
                        }

                        // Stop the queue internally via the PuzzleManager. We yield break so the coroutine ends now.
                        yield break; // Instantly stop movement
                    }
                }
            }

            float duration = distance / speed;
            Debug.Log($"[Command] Starting Movement: Moving from {startPosition} to {targetPosition}");
>>>>>>> Stashed changes

            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                droneTransform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        public string GetCommandName()
        {
            return $"Move Drone forward by {distance} units at {speed} units/sec";
        }
    }
}

// end of DroneMovementCommand.cs