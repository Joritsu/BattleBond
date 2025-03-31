using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("References")]
    public HealthBar healthBar;          // If you have a health bar
    public GameObject damagePopupPrefab; // The prefab for the popup
    public Canvas uiCanvas;             // Your Screen Space – Overlay canvas


    void Start()
    {
        currentHealth = maxHealth;

        // Update your HealthBar if you have one
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
    }

    void Update()
    {


    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0); // Don’t go below 0

        // Update the health bar
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
        if (currentHealth <= 0)
        {
            Die();
        }
        // Show the damage popup above the player.
        ShowDamagePopup(damage);

    }

    private void ShowDamagePopup(int damage)
    {
        // 1) Instantiate the popup as a child of the UICanvas
        GameObject popupObj = Instantiate(damagePopupPrefab, uiCanvas.transform, false);

        // 2) Grab the DamagePopup script from the new object
        DamagePopup popupScript = popupObj.GetComponent<DamagePopup>();
        if (popupScript != null)
        {
            // 3) Initialize the popup to follow THIS transform (the player)
            //    and to use our Screen Space – Overlay canvas.
            popupScript.InitializePopup(this.transform, uiCanvas, damage);
        }
    }

    // Public method to heal (optional)
    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        // Don’t exceed max health
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }


    // Method to handle when health reaches zero
    private void Die()
    {
        // You can play a death animation, drop loot, etc.
        Debug.Log(gameObject.name + " has perished!");

        // For simplicity, we just destroy this GameObject
        Destroy(gameObject);
    }
}
