using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    [Tooltip("The full-screen panel that shows when the player dies")]
    public GameObject gameOverPanel;

    [Tooltip("Button that returns the player to the Main Menu")]
    public Button menuButton;

    [Tooltip("Build index of your Main Menu scene")]
    public int mainMenuSceneIndex = 0;

    void Start()
    {
        // Make sure the panel is hidden at start
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // Wire up the button click to our public method
        if (menuButton != null)
        {
            // Remove any existing listeners, just in case
            menuButton.onClick.RemoveAllListeners();
            // Add our handler
            menuButton.onClick.AddListener(OnMenuButtonClicked);
        }
    }

    /// <summary>
    /// Call this from Health.Die() when the player Tag="Player" dies.
    /// </summary>
    public void ShowGameOver()
    {
        // Pause the game
        Time.timeScale = 0f;

        // Show the panel
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    /// <summary>
    /// Called by the Menu button on the GameOverPanel.
    /// </summary>
    public void OnMenuButtonClicked()
    {
        // Unpause
        Time.timeScale = 1f;
        // Load main menu
        SceneManager.LoadScene(mainMenuSceneIndex);
    }
}
