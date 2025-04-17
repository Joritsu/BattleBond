using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Tooltip("If set, this scene name will be loaded when enemies are cleared. Otherwise loads next in build order.")]
    public string nextSceneName;

    // Prevents loading repeatedly
    private bool transitionStarted = false;

    void Update()
    {
        // Only check if we haven’t already started the next‐level transition
        if (transitionStarted) return;
        
        // Find all active GameObjects tagged "Enemy"
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            transitionStarted = true;
            LoadNextLevel();
        }
    }

    private void LoadNextLevel()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            // no name provided → load next build‐index scene
            int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
            // wrap around or clamp if you like:
            if (nextIndex < SceneManager.sceneCountInBuildSettings)
                SceneManager.LoadScene(nextIndex);
            else
                Debug.LogWarning("No next scene in build settings.");
        }
    }
}
