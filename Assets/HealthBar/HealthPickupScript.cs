using UnityEngine;

public class HealthPickupScript : MonoBehaviour
{
    [Header("Healing Settings")]
    public int healAmount = 20;  // How much health to restore

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object has a Health script
        Health playerHealth = collision.GetComponent<Health>();
        if (playerHealth != null )
        {
            // Heal the player
            Debug.Log("DOes it work");
            playerHealth.Heal(healAmount);

            // Optional: Play a sound or particle effect here

            // Destroy this pickup so it can’t be used again
            Destroy(gameObject);
        }
    }
}
