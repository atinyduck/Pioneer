// ICommand.cs
// Written by:      Jake Morgan
// Last Updated:    15/03/2026

using System.Collections;
using UnityEngine;

namespace Pioneer.Commands
{
    /// <summary>
    /// Base interface for all executable commands in the game.
    /// Commands should represent discrete actions that can be queued and executed sequentially.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Execute the command asynchronously.
        /// Returns a coroutine that completes when the command finishes.
        /// </summary>
        IEnumerator Execute();
    }
}

// end of ICommand.cs