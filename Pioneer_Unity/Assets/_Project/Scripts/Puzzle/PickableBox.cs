// PickableBox.cs
// Written by:      Jake Morgan
// Last Updated:    21/04/2026

using UnityEngine;

namespace Pioneer.Puzzle
{
    // Identifies an object that can be picked up and droped by the drone
    public class PickableBox : MonoBehaviour
    {
        public bool IsHeld { get; set; } = false;
    }
}