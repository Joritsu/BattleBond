using UnityEngine;

public class Pause : MonoBehaviour
{
    private bool isPaused = false;

    // Call this method from the button's OnClick event.
    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f; // Stop all game movement and animations
        isPaused = true;
        // Optionally, show a pause menu or additional UI
        // jei kazkas darys pause menu sitoje vietoje reiktu ideti, kad atsirastu UwU

    }

    void ResumeGame()
    {
        Time.timeScale = 1f; // Resume normal game speed
        isPaused = false;
        // Optionally, hide the pause menu
    }
}
