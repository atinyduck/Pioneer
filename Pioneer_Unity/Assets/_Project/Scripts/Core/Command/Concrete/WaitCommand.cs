// WaitCommand.cs
// Written by:      Jake Morgan
// Last Updated:    15/03/2026

using System.Collections;
using UnityEngine;
using Pioneer.Commands;

namespace Pioneer.Commands.Concrete
{
    public class WaitCommand : ICommand
    {
        // --- FIELDS ---
        private readonly float duration;

        // --- CONSTRUCTOR ---
        public WaitCommand(float duration)
        {
            this.duration = duration;
        }

        // --- METHODS ---
        public IEnumerator Execute()
        {
            yield return new WaitForSeconds(duration);
        }

        public string GetCommandName()
        {
            return $"Wait for {duration} seconds";
        }
    }
}


// end of WaitCommand.cs