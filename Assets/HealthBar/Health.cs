using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip[] damageSounds; // Assign your 3+ damage sounds in the Inspector
    private AudioSource audioSource;

    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Reward (for enemies)")]
    [Tooltip("How much money the player gets when this unit dies.")]
    public int deathReward = 0;

    [Header("References")]
    public HealthBar healthBar;          // If you have a health bar
    public GameObject damagePopupPrefab; // The prefab for the popup
    public Canvas uiCanvas;             // Your Screen Space – Overlay canvas


    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
            healthBar.SetMaxHealth(maxHealth);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        if (healthBar != null)
            healthBar.SetHealth(currentHealth);
        ShowDamagePopup(amount, true);
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        if (healthBar != null)
            healthBar.SetHealth(currentHealth);
        ShowDamagePopup(damage, false);

        if (currentHealth <= 0)
            Die();

        if (damageSounds != null && damageSounds.Length > 0 && audioSource != null)
        {
            int randomIndex = Random.Range(0, damageSounds.Length);

            // Stop any currently playing sound
            audioSource.Stop();

            // Assign new clip and play
            audioSource.clip = damageSounds[randomIndex];
            audioSource.Play();
        }
    }

    void ShowDamagePopup(int amount, bool healing)
    {
        if (damagePopupPrefab == null || uiCanvas == null)
            return;

        GameObject popupObj = Instantiate(damagePopupPrefab, uiCanvas.transform, false);
        var popupScript = popupObj.GetComponent<DamagePopup>();
        if (popupScript != null)
            popupScript.InitializePopup(transform, uiCanvas, amount, healing);
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has perished!");

        // If this object is an enemy, award the player money:
        // You can use either Tag or Layer. Here’s Tag-based:
        if (CompareTag("Enemy") && PlayerMoney.Instance != null)
        {
            PlayerMoney.Instance.AddMoney(deathReward);
            Debug.Log($"Awarded {deathReward} for killing {gameObject.name}");
        }

        // If this is the *player* dying, trigger Game Over:
        if (CompareTag("Player"))
        {
            var gm = Object.FindFirstObjectByType<GameOverScript>();
            if (gm != null)
                gm.ShowGameOver();
        }

        Destroy(gameObject);
    }
}
