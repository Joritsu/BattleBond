using UnityEditor;
using UnityEngine;

public class ConfirmPanelYesNo : MonoBehaviour
{
    public GameObject confirmPanel;


    // Confirm button pressed:
    public void OnConfirmYes()
    {
        Debug.Log("Confirmed action. Quitting game.");

        // If Game is being Played on edditor close
        if (Application.isEditor)
        {
            // This will only work in the Editor; in a build this line causes an error.
            EditorApplication.isPlaying = false;
        }
        else // If game is being played normally
        {
            Application.Quit();
        }

        // Hide the confirmation panel after action
        confirmPanel.SetActive(false);
    }

    // Called when the user clicks the Cancel/No button
    public void OnConfirmNo()
    {
        confirmPanel.SetActive(false);
    }
}
