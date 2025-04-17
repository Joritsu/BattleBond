using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    [System.Serializable]
    public class ShopItem
    {
        public string itemName;    // e.g. "DoubleJumpBoots"
        public int cost;           // price in player money
        public GameObject prefab;  // what to spawn next level
        public Sprite   icon;
    }

    [Header("Configure your shop items here")]
    public ShopItem[] items;

    // Internal state
    private bool[] purchased;
    private int nextLevelBuildIndex = -1;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            purchased = new bool[items.Length];
        }
        else Destroy(gameObject);
    }

    /// <summary>
    /// Attempt to buy item at index i.
    /// Returns true if purchase succeeded.
    /// </summary>
    public bool BuyItem(int i)
    {
        if (i < 0 || i >= items.Length || purchased[i]) return false;
        if (!PlayerMoney.Instance.SpendMoney(items[i].cost)) return false;
        purchased[i] = true;
        return true;
    }

    /// <summary>
    /// Returns whether item at index i has been bought.
    /// </summary>
    public bool IsPurchased(int i)
    {
        return i >= 0 && i < purchased.Length && purchased[i];
    }

    /// <summary>
    /// Call this before loading the shop to tell it which build‐index to load next.
    /// </summary>
    public void SetNextLevelByBuildIndex(int buildIndex)
    {
        nextLevelBuildIndex = buildIndex;
    }

    /// <summary>
    /// Called by your “Proceed” button in the Shop UI.
    /// Loads the next level using the build‐index previously set.
    /// </summary>
    public void ProceedToNextLevel()
    {
        Debug.Log($"[ShopManager] ProceedToNextLevel called. nextLevelBuildIndex = {nextLevelBuildIndex}");
        if (nextLevelBuildIndex >= 0 
            && nextLevelBuildIndex < SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log($"[ShopManager] Loading build index {nextLevelBuildIndex}");
            SceneManager.LoadScene(nextLevelBuildIndex);
        }
        else
        {
            Debug.LogError($"ShopManager: invalid nextLevelBuildIndex = {nextLevelBuildIndex}");
        }
    }


}
