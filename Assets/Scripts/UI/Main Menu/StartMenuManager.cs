using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartMenuManager : MonoBehaviour
{
    public GameObject confirmPanel;
    // This method will be called when the Start button is clicked.
    // Main menu
    public void StartGame()
    {
        // Loads specific scene when button is pressed
        SceneManager.LoadScene("Main");
    }

    public void Settings()
    {
        // Loads specific scene when button is pressed
        SceneManager.LoadScene("Settings");
    }

    // This method will be called when the Quit button is clicked.
    public void QuitGame()
    {
        // Turns on Confirmation Pannel
        confirmPanel.SetActive(true);

    }



    // xoxo chatgpt (. Y .) 
}
