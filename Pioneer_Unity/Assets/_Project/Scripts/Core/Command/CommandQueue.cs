// CommandQueue.cs
// Written by:      Jake Morgan
// Last Updated:    15/03/2026

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pioneer.Commands;

public class CommandQueue : MonoBehaviour
{
    // --- FIELDS ---
    private Queue<ICommand> commandQueue = new Queue<ICommand>();
    private List<ICommand> commandHistory = new List<ICommand>();
    private Coroutine executionCoroutine; 
    private bool isPaused = false;
    private bool isExecuting = false;

    // --- PROPERTIES ---
    public int CommandCount => commandQueue.Count;
    public int CommandHistoryCount => commandHistory.Count;
    public bool IsPaused => isPaused;
    public bool IsExecuting => isExecuting;
    public ICommand CurrentCommand => isExecuting && commandQueue.Count > 0 ? commandQueue.Peek() : null;
    
    // --- EVENTS ---
    public delegate void CommandDelegate(ICommand command);
    public event System.Action OnQueueStarted;
    public event CommandDelegate OnCommandStarted;
    public event CommandDelegate OnCommandFinished;
    public event System.Action OnQueueEmpty;

    // --- LIFECYCLE ---

    /// <summary>
    /// Starts executing all queued commands sequentially.
    /// If already executing, this will log a warning and return.
    /// </summary>
    public void Execute()
    {
        if (isExecuting)
        {
            Debug.LogWarning("Commands are already executing. Call Stop() first.");
            return;
        }

        if (commandQueue.Count == 0)
        {
            Debug.LogWarning("No commands in queue to execute.");
            return;
        }

        executionCoroutine = StartCoroutine(ExecuteCommandsCoroutine());
    }

    /// <summary>
    /// Adds a command to the end of the queue. Commands will be executed in the order they were added.
    /// </summary>
    /// <param name="command">The command to be enqueued.</param>
    public void EnqueueCommand(ICommand command)
    {
        if (command == null)
        {
            Debug.LogError("Cannot enqueue null command");
            return;
        }
        
        commandQueue.Enqueue(command);
        Debug.Log($"Command enqueued. Queue size: {commandQueue.Count}");
    }

    /// <summary>
    /// Removes the command at the front of the queue. This is typically called after a command has finished executing.
    /// </summary>
    public void DequeueCommand()
    {
        
        if (commandQueue.Count > 0)
        {
            commandQueue.Dequeue();
            Debug.Log($"Command dequeued. Queue size: {commandQueue.Count}");
        }
    }

    /// <summary>
    /// Pauses the execution of commands. The current command will finish executing, but no new commands will start until Resume() is called.
    /// </summary>
    public void Pause()
    {
        if (isExecuting && !isPaused)
        {
            isPaused = true;
            Debug.Log("Command execution paused.");
        }
    }

    /// <summary>
    /// Resumes the execution of commands after being paused. If there are commands in the queue, execution will continue from the next command.
    /// </summary>
    public void Resume()
    {
        if (isPaused)
        {
            isPaused = false;
            Debug.Log("Command execution resumed.");
        }
    }

    /// <summary>
    /// Stops the execution of commands immediately.
    /// </summary>
    public void Stop()
    {
        if (executionCoroutine != null)
        {
            StopCoroutine(executionCoroutine);
            executionCoroutine = null;
        }

        commandQueue.Clear();
        isExecuting = false;
        isPaused = false;
        Debug.Log("Command execution stopped and queue cleared.");
    }

    /// <summary>
    /// Clears the command history. This does not affect the current queue of commands, but will remove all records of previously executed commands from the history list.
    /// </summary>
    public void ClearHistory ()
    {
        commandHistory.Clear();
    }

    /// <summary>
    /// Coroutine that manages the execution of commands in the queue. It will execute each command sequentially, waiting for each command to finish before starting the next one. If the queue is paused or empty, it will wait until there are commands to execute or until it is resumed.
    /// </summary>
    private IEnumerator ExecuteCommandsCoroutine() 
    { 
        isExecuting = true;
        OnQueueStarted?.Invoke();
        Debug.Log($"=== Queue Started ({CommandCount} commands) ===");

        int executionIndex = 1;
        while (commandQueue.Count > 0)
        {
            while (isPaused)
            {
                yield return null;
            }

            ICommand currentCommand = commandQueue.Peek();
            string commandType = currentCommand.GetType().Name;
            string commandDesc = currentCommand.GetCommandName();
            
            Debug.Log($"[{executionIndex}/{CommandHistoryCount + CommandCount}] {commandType}: {commandDesc}");
            OnCommandStarted?.Invoke(currentCommand);
            
            yield return StartCoroutine(currentCommand.Execute());
            
            OnCommandFinished?.Invoke(currentCommand);
            commandHistory.Add(currentCommand);
            commandQueue.Dequeue();
            
            executionIndex++;
        } 

        Debug.Log($"=== Queue Complete ({CommandHistoryCount} commands executed) ===");
        OnQueueEmpty?.Invoke();
        isExecuting = false;
        executionCoroutine = null;
    }
}
// end of CommandQueue.cs