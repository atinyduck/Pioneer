using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Pioneer.Integration.Brython;

namespace Pioneer.UI
{
    public class CodeEditorUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_InputField codeInputField;
        [SerializeField] private Button runButton;
        [SerializeField] private Button clearButton;
        [SerializeField] private TextMeshProUGUI statusText;

        [Header("System References")]
        [SerializeField] private BrythonRunner brythonRunner;

        private void Start()
        {
            // Set up button listeners
            if (runButton != null)
                runButton.onClick.AddListener(OnRunClicked);

            if (clearButton != null)
                clearButton.onClick.AddListener(OnClearClicked);

            // Set up default text to help the player
            if (codeInputField != null)
            {
                // Load previously saved code for this level if it exists
                string currentSceneName = SceneManager.GetActiveScene().name;
                string savedCode = PlayerPrefs.GetString($"SavedCode_{currentSceneName}", "");

                if (!string.IsNullOrEmpty(savedCode))
                {
                    codeInputField.text = savedCode;
                }
                else if (string.IsNullOrEmpty(codeInputField.text))
                {
                    codeInputField.text = "# Type your Python code here!\n# Example:\n# move()\n# turn(90)\n# pickup()\n# drop()";
                }

                // Unity natively does not allow modifying the Tab Size dynamically inside the InputField component natively.
                // We will trap the 'Tab' key press at the application level and manually replace it with 4 spaces
                // to completely mimic a standard Python IDLE text editor.
                codeInputField.onValidateInput += delegate (string input, int charIndex, char addedChar)
                {
                    if (addedChar == '\t')
                    {
                        // Replace literal tab keystrokes with exactly 4 spaces
                        codeInputField.text = input.Insert(charIndex, "    ");

                        // We must return a null character (the null char '\0') so Unity does not append
                        // the original tab to our new string!
                        codeInputField.caretPosition = charIndex + 4; 
                        return '\0'; 
                    }
                    return addedChar;
                };
            }

            UpdateStatus("Ready");
        }

        private void OnDestroy()
        {
            if (runButton != null) runButton.onClick.RemoveListener(OnRunClicked);
            if (clearButton != null) clearButton.onClick.RemoveListener(OnClearClicked);
        }

        private void OnRunClicked()
        {
            if (codeInputField == null || string.IsNullOrWhiteSpace(codeInputField.text))
            {
                UpdateStatus("<color=red>Error: No code to run.</color>");
                return;
            }

            string codeToRun = codeInputField.text;
            
            // Save the code before running it, so if the level crashes and reloads, the code is preserved
            string currentSceneName = SceneManager.GetActiveScene().name;
            PlayerPrefs.SetString($"SavedCode_{currentSceneName}", codeToRun);
            PlayerPrefs.Save();

            UpdateStatus("<color=yellow>Executing code...</color>");

            // Pass the code to the Brython Runner bridge
            if (brythonRunner != null)
            {
                // Disable button to prevent spamming
                runButton.interactable = false; 

                brythonRunner.ExecutePython(codeToRun);

                // Note: Re-enabling the button should ideally happen after the CommandQueue finishes.
                // For now, we just enable it after a short delay.
                Invoke(nameof(EnableRunButton), 1.5f); 
            }
            else
            {
                UpdateStatus("<color=red>Error: BrythonRunner is missing.</color>");
            }
        }

        private void OnClearClicked()
        {
            if (codeInputField != null)
            {
                codeInputField.text = "";
                UpdateStatus("Editor Cleared.");
            }
        }

        public void UpdateStatus(string message)
        {
            if (statusText != null)
            {
                statusText.text = message;
            }
        }

        private void EnableRunButton()
        {
            if (runButton != null)
                runButton.interactable = true;

            UpdateStatus("Ready");
        }
    }
}