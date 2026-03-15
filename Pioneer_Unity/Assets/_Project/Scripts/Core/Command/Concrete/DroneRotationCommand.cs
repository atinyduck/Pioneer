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
            this.speed = speed;
        }

        // --- METHODS ---
        public IEnumerator Execute()
        {
            return null;
        }
    }
}


// end of DroneRotationCommand.cs