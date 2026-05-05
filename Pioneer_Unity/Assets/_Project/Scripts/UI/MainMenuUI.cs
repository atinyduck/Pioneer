using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pioneer.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [Tooltip("The exact name or index of the first level scene")]
        public string firstLevelSceneName = "Level1";

        public void PlayGame()
        {
            Debug.Log("Starting Game...");
            SceneManager.LoadScene(firstLevelSceneName);
        }

        public void QuitGame()
        {
            Debug.Log("Quitting Game...");
            Application.Quit();
        }
    }
}