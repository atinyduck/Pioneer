using UnityEngine;
using System.Runtime.InteropServices;

namespace Pioneer.Integration.Brython
{
    public class BrythonRunner : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern void RunBrythonScript(string scriptText);

        [SerializeField] private BrythonCommandParser parser;

        /// <summary>
        /// Call this method from your UI Run Button, passing the text from the UI Input Field.
        /// </summary>
        public void ExecutePython(string pythonCode)
        {
            // Secret debugging command to skip to the next level
            if (pythonCode.Trim().ToLower() == "skip()")
            {
                Debug.Log("[Cheat] Skipping to Next Level...");
                int currentBuildIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
                int totalScenes = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
                if (currentBuildIndex + 1 < totalScenes)
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(currentBuildIndex + 1);
                }
                else
                {
                    Debug.LogWarning("Already on the last level!");
                }
                return; // Return early, don't execute script
            }

            // If in the Unity Editor, WebGL Plugins don't work. We need a fallback or clear warning.
            #if UNITY_EDITOR
                Debug.LogWarning("Brython evaluation only works in the compiled WebGL build. Mocking response for testing.");
                MockExecutionForEditor(pythonCode);
            #elif UNITY_WEBGL
                RunBrythonScript(pythonCode);
            #else
                Debug.LogError("Brython Runner requires WebGL platform.");
            #endif
        }

        private void MockExecutionForEditor(string code)
        {
            // A slightly more robust mock parser so we can test sequences of commands in the Editor
            System.Text.StringBuilder jsonBuilder = new System.Text.StringBuilder();
            jsonBuilder.Append("{\"commands\": [");

            string[] lines = code.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
            bool isFirst = true;

            foreach (string line in lines)
            {
                string cleanLine = line.Trim().ToLower();

                if (cleanLine.StartsWith("move"))
                {
                    if (!isFirst) jsonBuilder.Append(",");
                    jsonBuilder.Append("{\"action\": \"move\", \"value\": 1}");
                    isFirst = false;
                }
                else if (cleanLine.StartsWith("turn"))
                {
                    if (!isFirst) jsonBuilder.Append(",");
                    jsonBuilder.Append("{\"action\": \"turn\", \"value\": 90}");
                    isFirst = false;
                }
                else if (cleanLine.StartsWith("pickup") || cleanLine.StartsWith("drop"))
                {
                    if (!isFirst) jsonBuilder.Append(",");
                    jsonBuilder.Append("{\"action\": \"interact\", \"value\": 0}");
                    isFirst = false;
                }
            }

            jsonBuilder.Append("]}");

            parser.ParseAndEnqueueCommands(jsonBuilder.ToString());
        }
    }
}