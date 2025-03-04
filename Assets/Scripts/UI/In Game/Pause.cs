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


    void Update()
    {
        // Check if the Escape key was pressed this frame
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Place your code here - this code will run when Esc is pressed.
            Debug.Log("Escape key pressed!");

            // For example, toggle a pause menu:
            TogglePause();
        }
    }
    public void ResumeGame()
    {
        InGameMenuPanel.SetActive(false);
        PauseButton.SetActive(true);
        Time.timeScale = 1f; // Resume normal game speed
        // Optionally, hide the pause menu
    }
}
