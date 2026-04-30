using UnityEngine;
using TMPro;

namespace Pioneer.UI
{
    public class LevelInfoDisplay : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI contentText;

        [Header("Data Source")]
        [Tooltip("Drag a .txt file here to load the level's educational text.")]
        [SerializeField] private TextAsset levelInfoFile;

        private void Start()
        {
            LoadInfo();
        }

        public void LoadInfo()
        {
            if (levelInfoFile != null && titleText != null && contentText != null)
            {
                string fullText = levelInfoFile.text;

                // Simple trick: The first line of the text file becomes the Title.
                // The rest of the file becomes the content.
                int firstNewline = fullText.IndexOf('\n');

                if (firstNewline > 0)
                {
                    titleText.text = fullText.Substring(0, firstNewline).Trim();
                    contentText.text = fullText.Substring(firstNewline).Trim();
                }
                else
                {
                    titleText.text = "Level Overview";
                    contentText.text = fullText;
                }
            }
            else
            {
                Debug.LogWarning("[LevelInfoDisplay] Missing UI references or Text Asset.");
                if (titleText != null) titleText.text = "Missing Info";
                if (contentText != null) contentText.text = "Please assign a .txt file to the LevelInfoDisplay script.";
            }
        }
    }
}