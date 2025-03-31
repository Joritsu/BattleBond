using UnityEngine;

public class DealDamageOnCollision : MonoBehaviour
{
    [SerializeField] private int damageAmount = 5;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object has a HealthSystem
        Health health = other.GetComponent<Health>();
        if (health != null)
        {
            // Apply damage
            health.TakeDamage(damageAmount);
        }
    }
}
