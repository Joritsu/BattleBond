// InputBindings.cs
using UnityEngine;
using System.Collections.Generic;

public static class InputBindings
{
    private static Dictionary<string, KeyCode> _bindings;

    // call once at startup
    public static void Initialize()
    {
        if (_bindings != null) return;

        _bindings = new Dictionary<string, KeyCode>()
        {
            { "Jump",      (KeyCode)PlayerPrefs.GetInt("JumpKey",      (int)KeyCode.Space) },
            { "MoveLeft",  (KeyCode)PlayerPrefs.GetInt("MoveLeftKey",  (int)KeyCode.A)     },
            { "MoveRight", (KeyCode)PlayerPrefs.GetInt("MoveRightKey", (int)KeyCode.D)     },
        };
    }

    public static KeyCode GetKey(string action)
    {
        Initialize();
        if (_bindings.TryGetValue(action, out var kc)) return kc;
        return KeyCode.None;
    }

    public static void SetKey(string action, KeyCode kc)
    {
        Initialize();
        _bindings[action] = kc;
        PlayerPrefs.SetInt(action + "Key", (int)kc);
        PlayerPrefs.Save();
    }
}
