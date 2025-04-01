using UnityEngine;
using UnityEngine.UI;

public class ok : MonoBehaviour
{
    // Reference to the disclaimer panel (assign via Inspector)
    public GameObject disclaimerPanel;

    // Reference to the OK button (assign via Inspector)
    public Button okButton;

    private void Start()
    {
        // Ensure the disclaimer panel is active on scene start
        if (disclaimerPanel != null)
            disclaimerPanel.SetActive(true);

        // Add a listener to the OK button so that it hides the panel when clicked
        if (okButton != null)
            okButton.onClick.AddListener(HideDisclaimer);
    }

    // Method called when OK is clicked
    public void HideDisclaimer()
    {
        // Hide the disclaimer panel
        
        disclaimerPanel.SetActive(false);
        
    }
}