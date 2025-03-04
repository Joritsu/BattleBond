using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour
{

      // public GameObject InGameMenuPanel;



    public void ButtonMainMenu()
    {
      //InGameMenuPanel.SetActive(false);
        SceneManager.LoadScene("Start Menu");
    }
}
