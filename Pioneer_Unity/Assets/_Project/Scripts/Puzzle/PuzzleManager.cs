// PuzzleManager.cs
// Written by:      Jake Morgan
// Last Updated:    24/04/2026

using UnityEngine;
using Pioneer.Commands;
using Pioneer.UI;
using System.Collections.Generic;

namespace Pioneer.Puzzle
{
    public class PuzzleManager : MonoBehaviour
    {
        // --- STATE STRUCTS ---
        private struct BoxState
        {
            public PickableBox box;
            public Vector3 startPosition;
            public Quaternion startRotation;
            public Transform startParent;
            public bool startIsHeld;
        }

        // --- FIELDS ---
        [Header("System References")]
        [SerializeField] private CommandQueue commandQueue;
        [SerializeField] private PuzzleValidator currentValidator;
        [SerializeField] private FeedbackManager feedbackManager;

        [Header("State Tracking")]
        [SerializeField] private Transform droneTransform;
        
        // State tracking for resets
        private Vector3 initialDronePosition;
        private Quaternion initialDroneRotation;
        private List<BoxState> initialBoxStates = new List<BoxState>();

        // --- LIFECYCLE ---
        private void Start()
        {
            // 1. Record starting state of the Drone
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

            // 2. Record starting state of all Pickable Boxes in the scene
            PickableBox[] allBoxes = FindObjectsByType<PickableBox>(FindObjectsSortMode.None);
            foreach (PickableBox b in allBoxes)
            {
                initialBoxStates.Add(new BoxState
                {
                    box = b,
                    startPosition = b.transform.position,
                    startRotation = b.transform.rotation,
                    startParent = b.transform.parent,
                    startIsHeld = b.IsHeld
                });
            }

            // 3. Subscribe to the Queue's finish event
            if (commandQueue != null)
            {
                commandQueue.OnQueueEmpty += HandleQueueFinished;
            }

            // 4. Subscribe to the Validator's win/loss events
            if (currentValidator != null)
            {
                currentValidator.OnPuzzlePassed.AddListener(HandlePuzzlePassed);
                currentValidator.OnPuzzleFailed.AddListener(HandlePuzzleFailed);
            }
        }

        private void OnDestroy()
        {
            // Clean up event subscriptions
            if (commandQueue != null)
            {
                commandQueue.OnQueueEmpty -= HandleQueueFinished;
            }
            if (currentValidator != null)
            {
                currentValidator.OnPuzzlePassed.RemoveListener(HandlePuzzlePassed);
                currentValidator.OnPuzzleFailed.RemoveListener(HandlePuzzleFailed);
            }
        }

        // --- METHODS ---
        private void HandleQueueFinished()
        {
            Debug.Log("[PuzzleManager] Queue empty. Asking Validator to check state...");
            if (currentValidator != null)
            {
                currentValidator.CheckWinCondition(); 
            }
            else 
            {
                Debug.LogWarning("[PuzzleManager] No PuzzleValidator assigned! Cannot check win condition.");
            }
        }

        private void HandlePuzzlePassed()
        {
            Debug.Log("[PuzzleManager] Notification received: Puzzle Passed! Loading UI...");
            if (feedbackManager != null) feedbackManager.ShowSuccess();
        }

        private void HandlePuzzleFailed()
        {
            Debug.Log("[PuzzleManager] Notification received: Puzzle Failed! Suggesting Reset...");
            if (feedbackManager != null) feedbackManager.ShowFailure();
        }

        public void ResetPuzzle()
        {
            // Stop execution
            if (commandQueue != null) commandQueue.Stop();

            // Reset drone
            if (droneTransform != null)
            {
                droneTransform.position = initialDronePosition;
                droneTransform.rotation = initialDroneRotation;
            }
            
            // Loop through and reset all PickableBoxes
            foreach (BoxState state in initialBoxStates)
            {
                if (state.box != null)
                {
                    state.box.transform.SetParent(state.startParent);
                    state.box.transform.position = state.startPosition;
                    state.box.transform.rotation = state.startRotation;
                    state.box.IsHeld = state.startIsHeld;   
                }
            }
            
            // Hide UI
            if (feedbackManager != null) feedbackManager.HideAll();
            
            Debug.Log("[PuzzleManager] Level reset to initial state.");
        }
    }
}