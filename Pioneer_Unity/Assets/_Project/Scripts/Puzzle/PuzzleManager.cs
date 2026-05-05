// PuzzleManager.cs
// Written by:      Jake Morgan
// Last Updated:    24/04/2026

using UnityEngine;
using UnityEngine.SceneManagement;
using Pioneer.Commands;
using System.Collections.Generic;

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
        public bool startIsKinematic;
        public bool startDetectCollisions;
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
        Debug.Log("[PuzzleManager] Initializing...");

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
                Debug.Log("[PuzzleManager] Auto-found Drone!");
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
            Rigidbody boxRb = b.GetComponent<Rigidbody>();
            initialBoxStates.Add(new BoxState
            {
                box = b,
                startPosition = b.transform.position,
                startRotation = b.transform.rotation,
                startParent = b.transform.parent,
                startIsHeld = b.IsHeld,
                startIsKinematic = boxRb != null && boxRb.isKinematic,
                startDetectCollisions = boxRb != null && boxRb.detectCollisions
            });
        }

        // 3. Subscribe to the Queue's finish event
        if (commandQueue != null)
        {
            Debug.Log($"[PuzzleManager] CommandQueue found: {commandQueue.gameObject.name}");
            commandQueue.OnQueueEmpty += HandleQueueFinished;
            Debug.Log("[PuzzleManager] Subscribed to CommandQueue OnQueueEmpty event");
        }
        else
        {
            Debug.LogError("[PuzzleManager] CommandQueue reference is NULL! THIS IS THE PROBLEM - assign it in the Inspector!");
        }

        // 4. Subscribe to the Validator's win/loss events
        if (currentValidator != null)
        {
            currentValidator.OnPuzzlePassed.AddListener(HandlePuzzlePassed);
            currentValidator.OnPuzzleFailed.AddListener(HandlePuzzleFailed);
            Debug.Log("[PuzzleManager] Subscribed to PuzzleValidator events");
        }
        else
        {
            Debug.LogError("[PuzzleManager] PuzzleValidator (currentValidator) reference is NULL!");
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
            Debug.LogError("[PuzzleManager] Cannot check win condition - currentValidator is NULL!");
        }
    }

    private void HandlePuzzlePassed()
    {
        Debug.Log("[PuzzleManager] ✅ PUZZLE PASSED! Showing success UI...");
        if (feedbackManager != null)
        {
            feedbackManager.ShowSuccess();
        }
        else
        {
            Debug.LogError("[PuzzleManager] FeedbackManager is NULL - cannot show success UI!");
        }
    }

    private void HandlePuzzleFailed()
    {
        Debug.Log("[PuzzleManager] ❌ PUZZLE FAILED! Suggesting Reset...");
        if (feedbackManager != null)
        {
            feedbackManager.ShowFailure();
        }
        else
        {
            Debug.LogError("[PuzzleManager] FeedbackManager is NULL - cannot show failure UI!");
        }
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

                Rigidbody boxRb = state.box.GetComponent<Rigidbody>();
                if (boxRb != null)
                {
                    boxRb.isKinematic = state.startIsKinematic;
                    boxRb.detectCollisions = state.startDetectCollisions;
                }
            }
        }

        // Hide UI
        if (feedbackManager != null) feedbackManager.HideAll();

        Debug.Log("[PuzzleManager] Level reset to initial state.");
    }

    public void LoadNextLevel()
    {
        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;

        Debug.Log($"[PuzzleManager.LoadNextLevel] Current scene index: {currentBuildIndex}, Total scenes: {totalScenes}");

        if (currentBuildIndex + 1 < totalScenes)
        {
            int nextSceneIndex = currentBuildIndex + 1;
            string nextSceneName = SceneManager.GetSceneByBuildIndex(nextSceneIndex).name;
            Debug.Log($"[PuzzleManager.LoadNextLevel] Loading scene {nextSceneIndex} ({nextSceneName})...");
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("[PuzzleManager] Already on the last level. No next scene to load.");
        }
    }
}
