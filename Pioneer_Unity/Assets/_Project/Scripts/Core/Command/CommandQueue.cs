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
    void Start() { }
    
    void Update() { }

    /// <summary>
    /// Adds a command to the end of the queue. Commands will be executed in the order they were added.
    /// </summary>
    /// <param name="command">The command to be enqueued.</param>
    public void EnqueueCommand(ICommand command)
    {
        commandQueue.Enqueue(command);
        if (!isExecuting)
        {
            executionCoroutine = StartCoroutine(ExecuteCommandsCoroutine());
        }
    }

    /// <summary>
    /// Removes the command at the front of the queue. This is typically called after a command has finished executing.
    /// </summary>
    public void DequeueCommand()
    {
        if (commandQueue.Count > 0)
        {
            commandQueue.Dequeue();
        }
    }

    /// <summary>
    /// Pauses the execution of commands. The current command will finish executing, but no new commands will start until Resume() is called.
    /// </summary>
    public void Pause()
    {
        isPaused = true;
    }

    /// <summary>
    /// Resumes the execution of commands after being paused. If there are commands in the queue, execution will continue from the next command.
    /// </summary>
    public void Resume()
    {
        if (isPaused)
        {
            isPaused = false;
            if (!isExecuting && commandQueue.Count > 0)
            {
                executionCoroutine = StartCoroutine(ExecuteCommandsCoroutine());
            }
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
        isExecuting = false;
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

        while (commandQueue.Count > 0)
        {
            if (isPaused)
            {
                yield return null; // Wait until the next frame and check again
                continue;
            }

            ICommand currentCommand = commandQueue.Peek();
            OnCommandStarted?.Invoke(currentCommand);
            yield return StartCoroutine(currentCommand.Execute());
            OnCommandFinished?.Invoke(currentCommand);

            commandHistory.Add(currentCommand);
            DequeueCommand();
        }
    }
}

// end of CommandQueue.cs