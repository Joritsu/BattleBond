using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TMPro;

public class ControlsManager : MonoBehaviour
{
    public static ControlsManager Instance { get; private set; }

    [Header("Rebind Buttons & Labels")]
    public Button jumpRebindButton;
    public TextMeshProUGUI   jumpKeyLabel;
    
    public Button moveLeftRebindButton;
    public TextMeshProUGUI   moveLeftKeyLabel;
    
    public Button moveRightRebindButton;
    public TextMeshPro   moveRightKeyLabel;

    // internal map of action → KeyCode
    private Dictionary<string, KeyCode> _bindings;
    // the action we're currently waiting on the user to press
    private string _rebindingAction;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // load saved or default keys
            _bindings = new Dictionary<string, KeyCode>()
            {
                { "Jump",      (KeyCode)PlayerPrefs.GetInt("JumpKey",      (int)KeyCode.Space) },
                { "MoveLeft",  (KeyCode)PlayerPrefs.GetInt("MoveLeftKey",  (int)KeyCode.A)     },
                { "MoveRight", (KeyCode)PlayerPrefs.GetInt("MoveRightKey", (int)KeyCode.D)     },
            };
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        // Update labels initially
        UpdateAllLabels();

        // Wire up the buttons
        jumpRebindButton.onClick.AddListener  (() => BeginRebind("Jump"));
        moveLeftRebindButton.onClick.AddListener  (() => BeginRebind("MoveLeft"));
        moveRightRebindButton.onClick.AddListener (() => BeginRebind("MoveRight"));
    }

    void Update()
    {
        // If we're in rebinding mode, look for any key press
        if (_rebindingAction != null)
        {
            foreach (KeyCode kc in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(kc))
                {
                    // Save the new binding
                    _bindings[_rebindingAction] = kc;
                    PlayerPrefs.SetInt(_rebindingAction + "Key", (int)kc);
                    PlayerPrefs.Save();

                    // Refresh labels and exit rebinding mode
                    UpdateAllLabels();
                    _rebindingAction = null;
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Retrieve the current KeyCode for a given action.
    /// </summary>
    public KeyCode GetKey(string action)
    {
        if (_bindings.TryGetValue(action, out var kc))
            return kc;
        return KeyCode.None;
    }

    /// <summary>
    /// Begin listening for the next key press to bind to <paramref name="action"/>.
    /// </summary>
    private void BeginRebind(string action)
    {
        _rebindingAction = action;

        // Give immediate feedback in the UI
        switch (action)
        {
            case "Jump":      jumpKeyLabel.text     = "Press any key…"; break;
            case "MoveLeft":  moveLeftKeyLabel.text  = "Press any key…"; break;
            case "MoveRight": moveRightKeyLabel.text = "Press any key…"; break;
        }
    }

    /// <summary>
    /// Write all current bindings into the on-screen labels.
    /// </summary>
    private void UpdateAllLabels()
    {
        jumpKeyLabel.text      = _bindings["Jump"].ToString();
        moveLeftKeyLabel.text  = _bindings["MoveLeft"].ToString();
        moveRightKeyLabel.text = _bindings["MoveRight"].ToString();
    }
}
