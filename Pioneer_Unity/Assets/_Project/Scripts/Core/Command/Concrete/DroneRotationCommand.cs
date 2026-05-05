// DroneRotationCommand.cs
// Written by:      Jake Morgan
// Last Updated:    15/03/2026

using System.Collections;
using UnityEngine;
using Pioneer.Commands;

namespace Pioneer.Commands.Concrete
{
    public class DroneRotationCommand : ICommand
    {
        // --- FIELDS ---
        private readonly Transform droneTransform;
        private readonly float angle;
        private readonly float speed;

        // --- CONSTRUCTOR ---
        public DroneRotationCommand(Transform droneTransform, float angle, float speed)
        {
            this.droneTransform = droneTransform;
            this.angle = angle;
            this.speed = speed <= 0 ? speed : 180f; // Ensure speed is not zero to avoid division by zero, default to 180 degrees/sec if invalid
        }

        // --- METHODS ---
        public IEnumerator Execute()
        {
            float duration = Mathf.Abs(angle) / speed;
            Quaternion startRotation = droneTransform.rotation;
            Quaternion targetRotation = startRotation * Quaternion.Euler(0, angle, 0);

            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                droneTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        public string GetCommandName()
        {
            return $"Rotate Drone by {angle} degrees at {speed} degrees/sec";
        }
    }
}


// end of DroneRotationCommand.cs