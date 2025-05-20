using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Tooltip("Build‐index of your Shop scene")]
    public int shopSceneIndex = 2;

    [Tooltip("Build‐index of your Main Menu scene")]
    public int mainMenuSceneIndex = 0;

    [Tooltip("Build‐index of your last playable level.  We never go past this.")]
    public int maxLevelBuildIndex = 4;  // e.g. if your last level is at build-index 4

    [Header("Level Complete UI")]
    public GameObject levelCompletePanel;  // your “You Win / Level Complete” panel
    public Button nextButton;          // the panel’s “Next” button

    bool _levelFinished = false;

    void Start()
    {
        if (nextButton != null)
            nextButton.onClick.AddListener(OnNextPressed);
    }

    void Update()
    {
        if (_levelFinished) return;

        int idx = SceneManager.GetActiveScene().buildIndex;
        // skip MainMenu and Shop itself
        if (idx == mainMenuSceneIndex || idx == shopSceneIndex) return;

        // all enemies dead?
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            _levelFinished = true;
            levelCompletePanel?.SetActive(true);
        }
    }

    void OnNextPressed()
    {
        // hide the panel
        levelCompletePanel?.SetActive(false);

        int currentIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentIndex >= maxLevelBuildIndex)
        {
            // Reset the player’s money
            if (PlayerMoney.Instance != null)
                PlayerMoney.Instance.ResetMoney();

            // Reset shop purchases
            if (ShopManager.Instance != null)
                ShopManager.Instance.ResetPurchases();

            // Now load your Main Menu
            SceneManager.LoadScene(mainMenuSceneIndex);
            return;
        }

        // otherwise, shop → next level as before
        int desired = (currentIndex < shopSceneIndex)
                    ? shopSceneIndex + 1
                    : currentIndex + 1;

        int returnIndex = Mathf.Min(desired, maxLevelBuildIndex);

        Debug.Log($"[LevelManager] NextPressed: current={currentIndex}, shop={shopSceneIndex}, " +
                  $"desiredReturn={desired}, clampedReturn={returnIndex}");

        ShopManager.Instance.SetNextLevelByBuildIndex(returnIndex);
        SceneManager.LoadScene(shopSceneIndex);
    }
    
    
}
