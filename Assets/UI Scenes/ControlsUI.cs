// ControlsUI.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ControlsUI : MonoBehaviour
{
    [Header("Rebind Buttons & TMP Labels")]
    public Button          jumpRebindButton;
    public TextMeshProUGUI jumpKeyLabel;

    public Button          moveLeftRebindButton;
    public TextMeshProUGUI moveLeftKeyLabel;

    public Button          moveRightRebindButton;
    public TextMeshProUGUI moveRightKeyLabel;

    string _rebindingAction = null;

    void Start()
    {
        // make sure your data is loaded
        InputBindings.Initialize();

        // show them
        RefreshLabels();

        // wire up the buttons
        jumpRebindButton.onClick.AddListener  (() => BeginRebind("Jump"));
        moveLeftRebindButton.onClick.AddListener  (() => BeginRebind("MoveLeft"));
        moveRightRebindButton.onClick.AddListener (() => BeginRebind("MoveRight"));
    }

    void Update()
    {
        if (_rebindingAction != null)
        {
            // wait for any new key press
            foreach (KeyCode kc in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(kc))
                {
                    // save it
                    InputBindings.SetKey(_rebindingAction, kc);
                    _rebindingAction = null;
                    RefreshLabels();
                    break;
                }
            }
        }
    }

    void BeginRebind(string action)
    {
        Debug.Log("BeginRebind for "+action);

        _rebindingAction = action;
        switch (action)
        {
            case "Jump": jumpKeyLabel.text = "Press any key…"; break;
            case "MoveLeft": moveLeftKeyLabel.text = "Press any key…"; break;
            case "MoveRight": moveRightKeyLabel.text = "Press any key…"; break;
        }
        
        
        
    }

    void RefreshLabels()
    {
        jumpKeyLabel.text      = InputBindings.GetKey("Jump").ToString();
        moveLeftKeyLabel.text  = InputBindings.GetKey("MoveLeft").ToString();
        moveRightKeyLabel.text = InputBindings.GetKey("MoveRight").ToString();
    }
}
