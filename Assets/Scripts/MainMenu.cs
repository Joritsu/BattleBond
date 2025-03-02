using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene switching

public class MainMenu : MonoBehaviour
{
    // Quits the game
    public void QuitGame()
    {
        Debug.Log("Game Quit"); // Only logs in Unity Editor
        Application.Quit(); // Closes the application (only works in built version)
    }
}

