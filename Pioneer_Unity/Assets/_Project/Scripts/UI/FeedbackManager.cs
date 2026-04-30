// FeedbackManager.cs
// Written by:      Jake Morgan
// Last Updated:    24/04/2026

using UnityEngine;
using UnityEngine.SceneManagement;
using Pioneer.Puzzle;

namespace Pioneer.UI
{
    public class FeedbackManager : MonoBehaviour
    {
        // --- FIELDS ---
        [Header("System References")]
        [SerializeField] private PuzzleManager puzzleManager;

        [Header("UI Panels")]
        [Tooltip("The parent GameObject containing the Success UI elements")]
        [SerializeField] private GameObject successPanel;
        
        [Tooltip("The parent GameObject containing the Failure/Hint UI elements")]
        [SerializeField] private GameObject failurePanel;

        // --- LIFECYCLE ---
        private void Start()
        {
            // Ensure no panels are showing when the level begins
            HideAll();
        }

        // --- METHODS ---
        
        /// <summary>
        /// Displays the success screen and hides the failure screen.
        /// </summary>
        public void ShowSuccess()
        {
            HideAll();
            
            if (successPanel != null)
            {
                successPanel.SetActive(true);
            }
            else
            {
                Debug.LogWarning("[FeedbackManager] Success Panel not assigned in the Inspector!");
            }
        }

        /// <summary>
        /// Displays the failure/hint screen and hides the success screen.
        /// </summary>
        public void ShowFailure()
        {
            HideAll();
            
            if (failurePanel != null)
            {
                failurePanel.SetActive(true);
            }
            else
            {
                Debug.LogWarning("[FeedbackManager] Failure Panel not assigned in the Inspector!");
            }
        }

        /// <summary>
        /// Hides all feedback panels. Called on start and when resetting a level.
        /// </summary>
        public void HideAll()
        {
            if (successPanel != null) successPanel.SetActive(false);
            if (failurePanel != null) failurePanel.SetActive(false);
        }

        /// <summary>
        /// Hook this method up to the OnClick event of the Restart UI Button
        /// </summary>
        public void OnRestartClicked()
        {
            if (puzzleManager != null)
            {
                puzzleManager.ResetPuzzle();
                HideAll(); // Ensure we hide everything when restarting
            }
            else
            {
                Debug.LogWarning("[FeedbackManager] PuzzleManager reference missing! Cannot restart.");
            }
        }

        /// <summary>
        /// Hook this method up to the OnClick event of the Next Level UI Button
        /// </summary>
        public void OnNextLevelClicked(string nextSceneName)
        {
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogWarning("[FeedbackManager] Next scene name was not provided.");
            }
        }
    }
}
