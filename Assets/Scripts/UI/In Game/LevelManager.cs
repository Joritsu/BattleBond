using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Tooltip("Build‐index of your Shop scene")]
    public int shopSceneIndex = 2;

    [Tooltip("Build‐index of your last playable level.  We never go past this.")]
    public int maxLevelBuildIndex = 4;  // e.g. if your last level is at build‑index 4

    [Header("Level Complete UI")]
    public GameObject levelCompletePanel;  // your “You Win / Level Complete” panel
    public Button     nextButton;          // the panel’s “Next” button

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
        // skip MainMenu (0) and Shop itself
        if (idx == 0 || idx == shopSceneIndex) return;

        int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (enemyCount == 0)
        {
            _levelFinished = true;
            ShowLevelCompleteUI();
        }
    }

    void ShowLevelCompleteUI()
    {
        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(true);
    }

    void OnNextPressed()
    {
        // hide the panel
        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);

        int currentIndex = SceneManager.GetActiveScene().buildIndex;

        // compute desired return index:
        int desired;
        if (currentIndex < shopSceneIndex)
        {
            // e.g. from Level1 before shop → go to shopIndex+1
            desired = shopSceneIndex + 1;
        }
        else
        {
            // from any level after shop → just current+1
            desired = currentIndex + 1;
        }

        // clamp so we never exceed the last level
        int returnIndex = Mathf.Min(desired, maxLevelBuildIndex);

        Debug.Log($"[LevelManager] NextPressed: current={currentIndex}, shop={shopSceneIndex}, " +
                  $"desiredReturn={desired}, clampedReturn={returnIndex}");

        ShopManager.Instance.SetNextLevelByBuildIndex(returnIndex);
        SceneManager.LoadScene(shopSceneIndex);
    }
}
