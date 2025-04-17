using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Tooltip("If set, this scene name will be loaded when you click Next Level. Otherwise, uses build index.")]
    public string nextSceneName;

    [Tooltip("Drag your WinPanel GameObject here (the panel with 'You Won!' & button).")]
    public GameObject winPanel;

    bool _winTriggered = false;

    void Start()
    {
        // Ensure it's hidden at start
        if (winPanel != null)
            winPanel.SetActive(false);
    }

    void Update()
    {
        if (_winTriggered) return;

        // Count all active GameObjects tagged "Enemy"
        int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        Debug.Log($"[LevelManager] Enemy count: {enemyCount}");

        if (enemyCount == 0)
        {
            _winTriggered = true;
            ShowWinScreen();
        }
    }

    void ShowWinScreen()
    {
        if (winPanel != null)
        {
            Debug.Log("[LevelManager] All enemies gone! Showing Win Screen...");
            // Pause the game
            Time.timeScale = 0f;
            winPanel.SetActive(true);
        }
        else
        {
            // Fallback to auto‐load if no panel assigned
            LoadNextLevel();
        }
    }

    /// <summary>
    /// Called by the Next Level button (or fallback).
    /// </summary>
    public void LoadNextLevel()
    {
        // Unpause
        Time.timeScale = 1f;

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            Debug.Log($"[LevelManager] Loading scene '{nextSceneName}'");
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (nextIndex < SceneManager.sceneCountInBuildSettings)
            {
                Debug.Log($"[LevelManager] Loading scene build index {nextIndex}");
                SceneManager.LoadScene(nextIndex);
            }
            else
            {
                Debug.LogWarning("[LevelManager] No next scene in build settings.");
            }
        }
    }
}
