using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonScript : MonoBehaviour
{
    // This method is linked to the button click event
    public void StartGame()
    {
        Debug.Log("Start button clicked!");
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main");
    }
}
