using UnityEngine;

public class ObjectDamage : MonoBehaviour
{
    public int damage = 10;              // Damage dealt each tick
    public float damageInterval = 0.8f;    // Time (in seconds) between damage ticks

    // Dictionary to track time for each collider (if multiple objects might be damaged)
    private System.Collections.Generic.Dictionary<Collider2D, float> damageTimers =
        new System.Collections.Generic.Dictionary<Collider2D, float>();

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Try to get the Health component from the colliding object.
        Health health = collision.GetComponent<Health>();
        if (health != null || collision.CompareTag("Player"))
        {
            // If this collider is not tracked yet, add it.
            if (!damageTimers.ContainsKey(collision))
            {
                damageTimers[collision] = 0f;
            }

            // Accumulate time
            damageTimers[collision] += Time.deltaTime;

            // If the timer reaches the interval, apply damage and reset the timer.
            if (damageTimers[collision] >= damageInterval)
            {
                health.TakeDamage(damage);
                damageTimers[collision] = 0f;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Remove the collider from the timer tracking when it leaves the trigger.
        if (damageTimers.ContainsKey(collision))
        {
            damageTimers.Remove(collision);
        }
    }

}
