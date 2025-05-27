using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    public GameObject confirmPanel;

    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void Settings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT APP - Application.Quit() called directly");
        Application.Quit();

        // If running in the Unity Editor, Application.Quit() does not stop play mode.
        // This line will stop play mode in the editor.
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}