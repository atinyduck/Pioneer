// PuzzleValidator.cs
// Written by:      Jake Morgan
// Last Updated:    21/04/2026

using UnityEngine;

namespace Pioneer.Puzzle
{
    public class PuzzleValidator : MonoBehaviour
    {
        // --- FIELDS ---
        [Tooltip("The target position the drone needs to reach.")]
        [SerializeField] private Transform targetGoal;
        
        [Tooltip("How close the drone needs to be to the center of the goal to win.")]
        [SerializeField] private float acceptableDistance = 0.1f;

        // --- METHODS ---
        /// <summary>
        /// Checks if the provided drone transform is within the acceptable distance to the goal.
        /// </summary>
        public bool IsPuzzleSolved(Transform droneTransform)
        {
            if (targetGoal == null)
            {
                Debug.LogError("No Target Goal set in PuzzleValidator!");
                return false;
            }

            if (droneTransform == null) return false;

            float distance = Vector3.Distance(droneTransform.position, targetGoal.position);
            return distance <= acceptableDistance;
        }
    }
}
