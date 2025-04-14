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
        confirmPanel.SetActive(true);
    }
}