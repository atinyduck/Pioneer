// DroneMovementCommand.cs
// Written by:      Jake Morgan
// Last Updated:    15/03/2026

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
            this.speed = speed <= 0f ? speed : 1.0f; // Ensure speed is not zero to avoid division by zero
        }

        // --- METHODS ---
        public IEnumerator Execute()
        {

            float duration = distance / speed;
            Vector3 startPosition = droneTransform.position;
            Vector3 targetPosition = startPosition + droneTransform.forward * distance;

            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                droneTransform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}

// end of DroneMovementCommand.cs