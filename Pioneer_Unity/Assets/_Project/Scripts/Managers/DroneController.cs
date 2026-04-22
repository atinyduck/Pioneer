// DroneController.cs
// Written by:      Jake Morgan
// Last Updated:    2024-06-15

using UnityEngine;
using UnityEngine.InputSystem;
using Pioneer.Commands.Concrete;

[RequireComponent(typeof(CommandQueue))]
public class DroneController : MonoBehaviour
{
    // --- FIELDS ---
    [Header("Movement Settings")]
    [Tooltip("Distance to move forward in units")]
    [SerializeField] private float moveDistance = 5f;
    [Tooltip("Speed in units/sec")]
    [SerializeField] private float moveSpeed = 2f;

    [Header("Rotation Settings")]
    [Tooltip("Angle to rotate in degrees")]
    [SerializeField] private float rotationAngle = 90f;
    [Tooltip("Rotation speed in degrees/sec")]
    [SerializeField] private float rotationSpeed = 90f;

    private CommandQueue commandQueue;

    // --- LIFECYCLE ---
    private void Awake()
    {
        // Get the CommandQueue component attached to the drone
        commandQueue = GetComponent<CommandQueue>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Keyboard.current == null) return;

        // Press 'W' to queue Forward movement
        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            var moveForwardCommand = new DroneMovementCommand(transform, moveDistance, moveSpeed);
            commandQueue.EnqueueCommand(moveForwardCommand);
        }

        // Press 'A' to queue Left rotation (Positive Z rotation in Unity)
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            var rotateLeftCommand = new DroneRotationCommand(transform, rotationAngle, rotationSpeed);
            commandQueue.EnqueueCommand(rotateLeftCommand);
        }

        // Press 'D' to queue Right rotation (Negative Z rotation in Unity)
        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            var rotateRightCommand = new DroneRotationCommand(transform, -rotationAngle, rotationSpeed);
            commandQueue.EnqueueCommand(rotateRightCommand);
        }

        // Press 'E' to queue an Interaction
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            float interactionDuration = 0.5f; // Wait half a second while interacting
            float interactRadius = moveDistance * 0.5f; // Reasonable radius based on grid size
            var interactCommand = new DroneInteractCommand(transform, interactionDuration, interactRadius);
            commandQueue.EnqueueCommand(interactCommand);
        }

        // Executing the queue
        if (Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.enterKey.wasPressedThisFrame)
        {
            commandQueue.Execute();
        }

        // Stop
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            commandQueue.Stop();
        }

        // Press 'R' to Reset the level
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            // Find the active puzzle manager and tell it to reset
            var puzzleManager = FindFirstObjectByType<Pioneer.Puzzle.PuzzleManager>();
            if (puzzleManager != null)
            {
                puzzleManager.ResetPuzzle();
            }
        }
    }
}
