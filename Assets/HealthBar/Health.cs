using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;

    public HealthBar healthBar;

    public GameObject damagePopupPrefab;
    public Canvas worldSpaceCanvas;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }
        */
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        healthBar.SetHealth(currentHealth);

        

        // Show the damage popup above the player.
        ShowDamagePopup(damage);
    }

    public void ShowDamagePopup(float damage)
    {
        // Define the desired spawn position in world space (above the player)
        Vector3 worldPos = transform.position + new Vector3(0, 2, 0);

        // Convert the world space position to a position relative to the canvas
        Vector3 localPos = worldSpaceCanvas.transform.InverseTransformPoint(worldPos);

        // Instantiate the popup as a child of the world space canvas
        GameObject popup = Instantiate(damagePopupPrefab, worldSpaceCanvas.transform);

        // Set the popup's local position so it appears near the player
        RectTransform popupRect = popup.GetComponent<RectTransform>();
        popupRect.localPosition = localPos;

        // Set the damage text on the popup
        DamagePopup dp = popup.GetComponent<DamagePopup>();
        if (dp != null)
        {
            dp.SetDamage(damage);
        }
    }

}
