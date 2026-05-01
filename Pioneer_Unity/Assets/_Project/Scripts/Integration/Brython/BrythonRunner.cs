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
            // Editor fallback: parse direct function calls so the Python API can be tested without WebGL.
            System.Text.StringBuilder jsonBuilder = new System.Text.StringBuilder();
            jsonBuilder.Append("{\"commands\":[");

            string[] lines = code.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
            bool isFirst = true;

            foreach (string rawLine in lines)
            {
                string cleanLine = rawLine.Trim().ToLower();

                if (string.IsNullOrWhiteSpace(cleanLine) || cleanLine.StartsWith("#"))
                {
                    continue;
                }

                string action = null;
                float value = 0f;

                if (cleanLine.StartsWith("move("))
                {
                    action = "move";
                    value = ExtractFloatArgument(cleanLine, 1f);
                }
                else if (cleanLine.StartsWith("turn("))
                {
                    action = "turn";
                    value = ExtractFloatArgument(cleanLine, 90f);
                }
                else if (cleanLine.StartsWith("pickup("))
                {
                    action = "pickup";
                }
                else if (cleanLine.StartsWith("drop("))
                {
                    action = "drop";
                }

                if (action == null)
                {
                    continue;
                }

                if (!isFirst)
                {
                    jsonBuilder.Append(",");
                }

                jsonBuilder.Append("{\"action\":\"").Append(action).Append("\",\"value\":").Append(value.ToString(System.Globalization.CultureInfo.InvariantCulture)).Append("}");
                isFirst = false;
            }

            jsonBuilder.Append("]}");

            parser.ParseAndEnqueueCommands(jsonBuilder.ToString());
        }

        private static float ExtractFloatArgument(string line, float fallback)
        {
            int openParenIndex = line.IndexOf('(');
            int closeParenIndex = line.IndexOf(')', openParenIndex + 1);

            if (openParenIndex < 0 || closeParenIndex <= openParenIndex + 1)
            {
                return fallback;
            }

            string argumentText = line.Substring(openParenIndex + 1, closeParenIndex - openParenIndex - 1).Trim();
            if (float.TryParse(argumentText, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float parsedValue))
            {
                return parsedValue;
            }

            return fallback;
        }
    }
}