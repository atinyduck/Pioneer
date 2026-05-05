// PuzzleValidator.cs
// Written by:      Jake Morgan
// Last Updated:    24/04/2026

using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic; // Added for Lists

[RequireComponent(typeof(Collider))]
public class PuzzleValidator : MonoBehaviour
    {
        // --- FIELDS ---
        [Header("Events")]
        public UnityEvent OnPuzzlePassed;
        public UnityEvent OnPuzzleFailed;

        [Header("Level Prerequisites")]
        [Tooltip("List of buttons that MUST be pressed to solve the puzzle.")]
        [SerializeField] private List<FloorButton> requiredButtons = new List<FloorButton>();

        // Tracks if the drone is currently standing inside the collider
        private bool isDroneInZone = false;

        // --- LIFECYCLE ---
        
        // Unity calls this the exact frame something touches the Trigger
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isDroneInZone = true;
                Debug.Log("[Validator] Drone entered the Goal Zone.");
            }
        }

        // Unity calls this the exact frame something leaves the Trigger
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isDroneInZone = false;
                Debug.Log("[Validator] Drone left the Goal Zone.");
            }
        }

        // --- METHODS ---

        /// <summary>
        /// Call this method when the CommandQueue finishes executing.
        /// It will check if the drone ended up in the zone AND if the level rules are met.
        /// </summary>
        public void CheckWinCondition()
        {
            Debug.Log("[Validator] Checking Win Conditions...");
            Debug.Log($"[Validator] isDroneInZone = {isDroneInZone}");

            bool arePrerequisitesMet = CheckLevelPrerequisites();
            Debug.Log($"[Validator] arePrerequisitesMet = {arePrerequisitesMet}");

            if (isDroneInZone && arePrerequisitesMet)
            {
                Debug.Log("[Validator] ✅ SUCCESS! Drone is in the zone and all prerequisites met.");
                OnPuzzlePassed?.Invoke();
            }
            else
            {
                Debug.Log($"[Validator] ❌ FAILED! In Zone: {isDroneInZone} | Prereqs Met: {arePrerequisitesMet}");
                OnPuzzleFailed?.Invoke();
            }
        }

        /// <summary>
        /// Checks if all required buttons in the list are currently pressed down.
        /// </summary>
        private bool CheckLevelPrerequisites()
        {
            if (requiredButtons.Count == 0)
            {
                Debug.Log("[Validator] No required buttons - prerequisites automatically met!");
                return true;
            }
            
            // Loop through all required buttons
            foreach (FloorButton button in requiredButtons)
            {
                // If even one button is not pressed, prerequisites fail!
                if (!button.IsPressed)
                {
                    Debug.Log($"[Validator] Prerequisite Failed: '{button.gameObject.name}' is not pressed.");
                    return false; 
                }
            }

            Debug.Log("[Validator] All button prerequisites are met.");
            return true; // We made it through the loop, so all buttons are pressed!
        }
    }
