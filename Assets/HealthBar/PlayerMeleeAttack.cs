using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private string targetTag = "Enemy";
    // This ensures we only deal damage to objects tagged as "Enemy"

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object has the target tag
        if (other.CompareTag(targetTag))
        {
            // Get the HealthSystem on the collided object
            Health health = other.GetComponent<Health>();
            if (health != null)
            {
                // Apply damage
                health.TakeDamage(attackDamage);
                Debug.Log("Dealt " + attackDamage + " damage to " + other.gameObject.name);
            }
        }
    }
}
