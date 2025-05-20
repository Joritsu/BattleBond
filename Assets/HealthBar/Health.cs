using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip[] damageSounds; 
    private AudioSource audioSource;

    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Collision Damage (player only)")]
    [Tooltip("How much HP the player loses when colliding with an enemy.")]
    public int collisionDamage = 10;
    [Tooltip("Minimum seconds between repeated collision damage.")]
    public float collisionDamageCooldown = 2f;

    // tracks when we last took collision damage
    private float _lastCollisionDamageTime = -Mathf.Infinity;

    [Header("Reward (for enemies)")]
    public int deathReward = 0;

    [Header("References")]
    public HealthBar healthBar;
    public GameObject damagePopupPrefab;
    public Canvas uiCanvas;

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
        healthBar?.SetHealth(currentHealth);
        ShowDamagePopup(amount, true);
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        healthBar?.SetHealth(currentHealth);
        ShowDamagePopup(damage, false);

        PlayRandomDamageSound();

        if (currentHealth <= 0)
            Die();
    }

    private void PlayRandomDamageSound()
    {
        if (damageSounds != null && damageSounds.Length > 0 && audioSource != null)
        {
            int idx = Random.Range(0, damageSounds.Length);
            audioSource.Stop();
            audioSource.clip = damageSounds[idx];
            audioSource.Play();
        }
    }

    void ShowDamagePopup(int amount, bool healing)
    {
        if (damagePopupPrefab == null || uiCanvas == null)
            return;

        var popup = Instantiate(damagePopupPrefab, uiCanvas.transform, false);
        var script = popup.GetComponent<DamagePopup>();
        if (script != null)
            script.InitializePopup(transform, uiCanvas, amount, healing);
    }

    private void Die()
    {
        Debug.Log($"{name} has perished!");

        if (CompareTag("Enemy") && PlayerMoney.Instance != null)
        {
            PlayerMoney.Instance.AddMoney(deathReward);
            Debug.Log($"Awarded {deathReward} for killing {name}");
        }

        if (CompareTag("Player"))
        {
            // find the first GameOverScript in the scene
            GameOverScript gm = FindObjectOfType<GameOverScript>();
            if (gm != null)
                gm.ShowGameOver();
        }



        Destroy(gameObject);
    }

    // --- NEW: handle repeated collision damage ---

    private void OnCollisionEnter2D(Collision2D col)
    {
        TryDealCollisionDamage(col.collider);
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        TryDealCollisionDamage(col.collider);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        TryDealCollisionDamage(col);
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        TryDealCollisionDamage(col);
    }

    private void TryDealCollisionDamage(Collider2D other)
    {
        // only the player takes bump damage from enemies
        if (!CompareTag("Player") || !other.CompareTag("Enemy"))
            return;

        // have we waited long enough since last hit?
        if (Time.time >= _lastCollisionDamageTime + collisionDamageCooldown)
        {
            TakeDamage(collisionDamage);
            _lastCollisionDamageTime = Time.time;
        }
    }
}
