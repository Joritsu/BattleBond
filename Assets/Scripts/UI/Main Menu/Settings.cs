using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public void StartMenu()
    {
        SceneManager.LoadScene("Start Menu");
    }

    public void Controls()
    {
        SceneManager.LoadScene("Controls");
    }
}