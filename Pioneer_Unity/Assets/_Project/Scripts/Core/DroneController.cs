// DroneController.cs
// Written by:      Jake Morgan
// Last Updated:    24/04/2026

using UnityEngine;
using UnityEngine.InputSystem;
using Pioneer.Commands; 
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

    [Header("Interaction Settings")]
    [Tooltip("An empty GameObject attached as a child to the drone where boxes go")]
    [SerializeField] private Transform carryPoint;
    [Tooltip("How far forward the drone can reach to grab a box")]
    [SerializeField] private float interactDistance = 1.25f;

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
            Debug.Log("[Input] W Pressed -> Enqueuing Forward Move");
            var moveForwardCommand = new DroneMovementCommand(transform, moveDistance, moveSpeed);
            commandQueue.EnqueueCommand(moveForwardCommand);
        }

        // Press 'A' to queue Left rotation (Negative Y rotation)
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            Debug.Log("[Input] A Pressed -> Enqueuing Rotate Left");
            var rotateLeftCommand = new DroneRotationCommand(transform, -rotationAngle, rotationSpeed);
            commandQueue.EnqueueCommand(rotateLeftCommand);
        }

        // Press 'D' to queue Right rotation (Positive Y rotation)
        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            Debug.Log("[Input] D Pressed -> Enqueuing Rotate Right");
            var rotateRightCommand = new DroneRotationCommand(transform, rotationAngle, rotationSpeed);
            commandQueue.EnqueueCommand(rotateRightCommand);
        }

        // Press 'E' to queue an Interaction
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Debug.Log("[Input] E Pressed -> Enqueuing Interaction");
            if (this.carryPoint == null)
            {
                Debug.LogWarning("Carry Point is not assigned in the Drone Controller!");
            }
            else
            {
                DroneInteractCommand interactCommand = new DroneInteractCommand(this.transform, this.carryPoint, this.interactDistance);
                commandQueue.EnqueueCommand(interactCommand);
            }
        }

        // Executing the queue
        if (Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.enterKey.wasPressedThisFrame)
        {
            Debug.Log("[Input] Space/Enter Pressed -> Starting Execution Loop");
            commandQueue.Execute();
        }

        // Stop
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Debug.Log("[Input] Escape Pressed -> Stopping queue execution");
            commandQueue.Stop();
        }

        // Press 'R' to Reset the level
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            Debug.Log("[Input] R Pressed -> Requesting Level Reset from PuzzleManager");
            var puzzleManager = FindFirstObjectByType<Pioneer.Puzzle.PuzzleManager>();
            if (puzzleManager != null)
            {
                puzzleManager.ResetPuzzle();
            }
        }
    }
}
