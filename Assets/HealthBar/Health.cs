using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("References")]
    public HealthBar healthBar;          // If you have a health bar
    public GameObject damagePopupPrefab; // The prefab for the popup
    public Canvas uiCanvas;             // Your Screen Space � Overlay canvas


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

    
    // Add or verify a Heal method
    public void Heal(int amount)
    {
        Debug.Log("HEALING");
        currentHealth += amount;
        // Neheal daugiau nei max
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        Debug.Log("After Healing, currentHealth = " + currentHealth);
        healthBar.SetHealth(currentHealth);
        ShowDamagePopup(amount, true);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0); // Dont go below 0
        Debug.Log("After damage, currentHealth = " + currentHealth);
        // Update the health bar
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
        if (currentHealth <= 0)
        {
            Die();

        }
        ShowDamagePopup(damage, false);

    }

    private void ShowDamagePopup(int damage, bool healing)
    {
        // 1) Instantiate the popup as a child of the UICanvas
        GameObject popupObj = Instantiate(damagePopupPrefab, uiCanvas.transform, false);

        // 2) Grab the DamagePopup script from the new object
        DamagePopup popupScript = popupObj.GetComponent<DamagePopup>();
        if (popupScript != null)
        {
            // 3) Initialize the popup to follow THIS transform (the player)
            //    and to use our Screen Space � Overlay canvas.
            popupScript.InitializePopup(this.transform, uiCanvas, damage, healing);
        }
    }


    private void Die()
    {
        Debug.Log(gameObject.name + " has perished!");
        GameOverScript gm = FindObjectOfType<GameOverScript>();
        if (gm == null)
        {
            Debug.Log("GameOverScript not found!");
        }
        else
        {
            gm.ShowGameOver();
        }
        Destroy(gameObject);
    }
}
