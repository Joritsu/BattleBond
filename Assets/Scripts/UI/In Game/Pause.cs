using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject InGameMenuPanel;
    public GameObject PauseButton;


    // Call this method from the button's OnClick event.
    public void TogglePause()
    {

            InGameMenuPanel.SetActive(true); // show in game panel
            PauseButton.SetActive(false); // show in game panel
            Time.timeScale = 0f; // Stop all game movement and animations
    }


    public void ResumeGame()
    {
        InGameMenuPanel.SetActive(false);
        PauseButton.SetActive(true);
        Time.timeScale = 1f; // Resume normal game speed
        // Optionally, hide the pause menu
    }
}
