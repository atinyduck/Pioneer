// PuzzleManager.cs
// Written by:      Jake Morgan
// Last Updated:    21/04/2026

using UnityEngine;

namespace Pioneer.Puzzle
{
    [RequireComponent(typeof(PuzzleValidator))]
    public class PuzzleManager : MonoBehaviour
    {
        // --- FIELDS ---
        [Header("Scene References")]
        [SerializeField] private Transform droneTransform;
        [SerializeField] private CommandQueue commandQueue;

        private PuzzleValidator validator;
        
        // State tracking for resets
        private Vector3 initialDronePosition;
        private Quaternion initialDroneRotation;

        // --- LIFECYCLE ---
        private void Awake()
        {
            validator = GetComponent<PuzzleValidator>();
        }

        private void Start()
        {
            // 1. Record starting position/rotation for clean resets
            if (droneTransform != null)
            {
                initialDronePosition = droneTransform.position;
                initialDroneRotation = droneTransform.rotation;
            }
            else
            {
                // Attempt to auto-find the drone if not assigned
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    droneTransform = player.transform;
                    initialDronePosition = droneTransform.position;
                    initialDroneRotation = droneTransform.rotation;
                }
                else
                {
                    Debug.LogWarning("[PuzzleManager] Drone transform not assigned and no object tagged 'Player' was found.");
                }
            }

            // 2. Subscribe to the Queue's finish event
            if (commandQueue != null)
            {
                commandQueue.OnQueueEmpty += HandleQueueFinished;
            }
        }

        private void OnDestroy()
        {
            if (commandQueue != null)
            {
                commandQueue.OnQueueEmpty -= HandleQueueFinished;
            }
        }

        // --- METHODS ---
        private void HandleQueueFinished()
        {
            // 3. When the queue finishes, validate the puzzle
            if (validator.IsPuzzleSolved(droneTransform))
            {
                Debug.Log("PUZZLE SOLVED! The drone reached the target.");
                // TODO: Trigger UI FeedbackSystem success modal
            }
            else
            {
                Debug.Log("PUZZLE FAILED! The drone is not at the target.");
                // TODO: Trigger UI FeedbackSystem hint modal
            }
        }

        public void ResetPuzzle()
        {
            if (droneTransform == null) return;

            // Stop any running commands immediately
            commandQueue.Stop();

            // Snap back to exactly where the drone started
            droneTransform.position = initialDronePosition;
            droneTransform.rotation = initialDroneRotation;
            
            Debug.Log("Puzzle Reset. Drone back to start.");
        }
    }
}