using UnityEngine;
using Pioneer.Commands;
using Pioneer.Commands.Concrete;

namespace Pioneer.Integration.Brython
{
    public class BrythonCommandParser : MonoBehaviour
    {
        [SerializeField] private CommandQueue commandQueue;
        [SerializeField] private Transform droneTransform;
        [SerializeField] private Transform droneCarryPoint;

        [Header("Movement Settings")]
        [SerializeField] private float defaultSpeed = 2f;
        [SerializeField] private float defaultRotationSpeed = 180f;

        /// <summary>
        /// Parses a JSON array of commands returning from Brython and enqueues them.
        /// </summary>
        public void ParseAndEnqueueCommands(string jsonOutput)
        {
            if (string.IsNullOrEmpty(jsonOutput))
            {
                Debug.LogWarning("Received empty string from Brython.");
                return;
            }

            Debug.Log($"Raw output from Python: {jsonOutput}");

            try
            {
                BrythonResponseData response = JsonUtility.FromJson<BrythonResponseData>(jsonOutput);

                if (!string.IsNullOrEmpty(response.error))
                {
                    Debug.LogError($"Python Execution Error: {response.error}");

                    // Try to find the CodeEditor UI to display the error to the player
                    UI.CodeEditorUI editorUI = FindFirstObjectByType<UI.CodeEditorUI>();
                    if (editorUI != null)
                    {
                        editorUI.UpdateStatus($"<color=red>Syntax Error: {response.error}</color>");
                    }

                    return;
                }

                if (response.commands == null || response.commands.Count == 0)
                {
                    Debug.Log("No commands received in this execution.");
                    return;
                }

                // Clear queue first if we are writing a full new sequence block
                if (commandQueue.CommandCount > 0)
                {
                    commandQueue.Stop();
                }

                foreach (var cmd in response.commands)
                {
                    ICommand droneCommand = null;

                    string action = cmd.action?.ToLower();
                    switch (action)
                    {
                        case "move":
                        case "move_forward":
                            float distance = cmd.value == 0 ? 1f : cmd.value;
                            droneCommand = new DroneMovementCommand(droneTransform, distance, defaultSpeed);
                            break;

                        case "turn":
                        case "rotate":
                            float angle = cmd.value == 0 ? 90f : cmd.value;
                            droneCommand = new DroneRotationCommand(droneTransform, angle, defaultRotationSpeed);
                            break;

                        case "interact":
                            droneCommand = new DroneInteractCommand(droneTransform, droneCarryPoint);
                            break;

                        case "pickup":
                            droneCommand = new DroneInteractCommand(droneTransform, droneCarryPoint, DroneInteractCommand.InteractionMode.Pickup);
                            break;

                        case "drop":
                            droneCommand = new DroneInteractCommand(droneTransform, droneCarryPoint, DroneInteractCommand.InteractionMode.Drop);
                            break;

                        default:
                            Debug.LogWarning($"Unknown command action received from Python: {cmd.action}");
                            break;
                    }

                    if (droneCommand != null)
                    {
                        commandQueue.EnqueueCommand(droneCommand);
                    }
                }

                // Start the queue executing
                commandQueue.Execute();

            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to parse Brython JSON Output: {e.Message}");
            }
        }
    }
}