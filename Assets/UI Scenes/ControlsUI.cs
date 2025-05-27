using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ControlsUI : MonoBehaviour
{
    [Header("Rebind Buttons & TMP Labels")]
    public Button jumpRebindButton;
    public TextMeshProUGUI jumpKeyLabel;

    public Button moveLeftRebindButton;
    public TextMeshProUGUI moveLeftKeyLabel;

    public Button moveRightRebindButton;
    public TextMeshProUGUI moveRightKeyLabel;

    string _rebindingAction = null;

    void Awake()
    {
        Debug.Log($"ControlsUI Awake on GameObject: '{gameObject.name}'. Active in hierarchy: {gameObject.activeInHierarchy}. Script enabled: {this.enabled}", gameObject);
        
        // Check button references immediately in Awake
        if (jumpRebindButton == null) Debug.LogError("ControlsUI Awake: jumpRebindButton is NULL!", gameObject);
        if (moveLeftRebindButton == null) Debug.LogError("ControlsUI Awake: moveLeftRebindButton is NULL!", gameObject);
        if (moveRightRebindButton == null) Debug.LogError("ControlsUI Awake: moveRightRebindButton is NULL!", gameObject);
        
        if (jumpKeyLabel == null) Debug.LogError("ControlsUI Awake: jumpKeyLabel is NULL!", gameObject);
        if (moveLeftKeyLabel == null) Debug.LogError("ControlsUI Awake: moveLeftKeyLabel is NULL!", gameObject);
        if (moveRightKeyLabel == null) Debug.LogError("ControlsUI Awake: moveRightKeyLabel is NULL!", gameObject);
    }

    void Start()
    {
        Debug.Log($"ControlsUI Start on GameObject: '{gameObject.name}'. Active in hierarchy: {gameObject.activeInHierarchy}. Script enabled: {this.enabled}", gameObject);

        // Initialize InputBindings first
        InputBindings.Initialize();
        Debug.Log("ControlsUI Start: InputBindings.Initialize() called.", gameObject);

        // Refresh labels
        RefreshLabels();
        Debug.Log("ControlsUI Start: RefreshLabels() called.", gameObject);

        // Wire up the buttons
        SetupButtonListener(jumpRebindButton, "Jump", "jumpRebindButton");
        SetupButtonListener(moveLeftRebindButton, "MoveLeft", "moveLeftRebindButton");
        SetupButtonListener(moveRightRebindButton, "MoveRight", "moveRightRebindButton");
    }

    void SetupButtonListener(Button button, string actionName, string buttonFieldName)
    {
        if (button != null)
        {
            Debug.Log($"ControlsUI Start: Button '{buttonFieldName}' ({button.gameObject.name}) is assigned. Interactable: {button.interactable}. Adding listener for action '{actionName}'.", button.gameObject);
            button.onClick.RemoveAllListeners(); // Clear previous listeners
            button.onClick.AddListener(() => BeginRebind(actionName));
        }
        else
        {
            Debug.LogError($"ControlsUI Start: Button field '{buttonFieldName}' is NULL! Cannot add listener for action '{actionName}'.", gameObject);
        }
    }

    void OnDestroy()
    {
        Debug.Log($"ControlsUI OnDestroy on GameObject: '{gameObject.name}'.", gameObject);
        // Clean up listeners when the object is destroyed
        if (jumpRebindButton != null) jumpRebindButton.onClick.RemoveAllListeners();
        if (moveLeftRebindButton != null) moveLeftRebindButton.onClick.RemoveAllListeners();
        if (moveRightRebindButton != null) moveRightRebindButton.onClick.RemoveAllListeners();
    }

    void Update()
    {
        if (_rebindingAction != null)
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode kc in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(kc))
                    {
                        if (kc >= KeyCode.Mouse0 && kc <= KeyCode.Mouse6) continue;
                        if (kc == KeyCode.None) continue;

                        Debug.Log($"Update: Detected KeyCode: {kc} for action '{_rebindingAction}'");
                        InputBindings.SetKey(_rebindingAction, kc);
                        _rebindingAction = null;
                        RefreshLabels();
                        break; 
                    }
                }
            }
        }
    }

    void BeginRebind(string action)
    {
        // THIS IS THE CRITICAL LOG YOU'RE MISSING ON THE SECOND ATTEMPT
        Debug.Log($"BeginRebind for action: '{action}' on GameObject: '{gameObject.name}'", gameObject);

        _rebindingAction = action;
        switch (action)
        {
            case "Jump":      jumpKeyLabel.text = "Press any key…"; break;
            case "MoveLeft":  moveLeftKeyLabel.text = "Press any key…"; break;
            case "MoveRight": moveRightKeyLabel.text = "Press any key…"; break;
        }
    }

    void RefreshLabels()
    {
        Debug.Log("ControlsUI RefreshLabels: Attempting to refresh labels.", gameObject);
        if (jumpKeyLabel == null || moveLeftKeyLabel == null || moveRightKeyLabel == null) {
            Debug.LogError("ControlsUI RefreshLabels: One or more key labels are null!", gameObject);
            return;
        }

        jumpKeyLabel.text = InputBindings.GetKey("Jump") != KeyCode.None ? InputBindings.GetKey("Jump").ToString() : "N/A";
        moveLeftKeyLabel.text = InputBindings.GetKey("MoveLeft") != KeyCode.None ? InputBindings.GetKey("MoveLeft").ToString() : "N/A";
        moveRightKeyLabel.text = InputBindings.GetKey("MoveRight") != KeyCode.None ? InputBindings.GetKey("MoveRight").ToString() : "N/A";
        Debug.Log("ControlsUI RefreshLabels: Labels refreshed.", gameObject);
    }
}