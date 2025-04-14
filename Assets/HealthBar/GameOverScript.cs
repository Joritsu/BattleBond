using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverScript : MonoBehaviour
{
    [Header("Game Over UI")]
    // This should be the parent panel containing the black background and the game over image
    public GameObject gameOverPanel;

    // This method enables the game over UI
    public void ShowGameOver()
    {
        // Make sure the panel is disabled in the Inspector initially
        gameOverPanel.SetActive(true);

        // Optional: freeze the game so nothing else moves/updates
        //Time.timeScale = 0f;
    }

    // This method (optional) hides the game over UI if needed
    public void HideGameOver()
    {
        gameOverPanel.SetActive(false);
        // Time.timeScale = 1f; // un-freeze the game if you froze it
    }

    public void RestartButton()
    {
        HideGameOver();
        SceneManager.LoadScene("Health_2.0");
    }    
    public void MainMenu()
    {
        SceneManager.LoadScene("Settings");
    }  
}
