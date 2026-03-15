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
            this.speed = speed;
        }

        // --- METHODS ---
        public IEnumerator Execute()
        {
            return null;
        }
    }
}

// end of DroneMovementCommand.cs