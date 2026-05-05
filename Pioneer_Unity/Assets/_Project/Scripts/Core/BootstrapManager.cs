using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pioneer.Core
{
    public class BootstrapManager : MonoBehaviour
    {
        [Tooltip("The exact name of the Main Menu scene to load after initialization.")]
        public string mainMenuSceneName = "MainMenu";

        private void Start()
        {
            // Initialize any core systems here (e.g., Save Systems, Audio Managers, Analytics)
            Debug.Log("Bootstrap: Initializing core systems...");

            // Load the Main Menu
            Invoke(nameof(LoadMainMenu), 1.0f); // Small delay just to show a splash screen if you want one
        }

        private void LoadMainMenu()
        {
            Debug.Log("Bootstrap: Initialization complete. Loading Main Menu...");
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }
}