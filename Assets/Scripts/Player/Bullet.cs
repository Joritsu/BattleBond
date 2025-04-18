using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Damage & Knockback")]
    [SerializeField] private int damage = 1;
    [Tooltip("Impulse force applied to the target on hit")]
    [SerializeField] private float knockbackForce = 5f;
    [Tooltip("How long the target is stunned (no AI) after knockback")]
    [SerializeField] private float knockbackStunDuration = 0.25f;

    [Header("Flight")]
    private Vector2 direction;
    public float speed = 10f;

    public void SetDirection(Vector2 dir) 
        => direction = dir.normalized;

    public void SetDamage(int dmg) 
        => damage = dmg;

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // only hurt enemies
        if (other.CompareTag("Enemy"))
        {
            // 1) Damage
            var health = other.GetComponent<Health>();
            if (health != null)
                health.TakeDamage(damage);

            // 2) Knockback + stun
            var enemyRb = other.attachedRigidbody;
            if (enemyRb != null)
            {
                // impulse goes along bullet’s travel direction
                Vector2 impulse = direction * knockbackForce;

                var ec = enemyRb.GetComponent<EnemyController>();
                if (ec != null)
                    ec.ApplyKnockback(impulse, knockbackStunDuration);
                else
                    enemyRb.AddForce(impulse, ForceMode2D.Impulse);
            }

            // 3) Destroy bullet on hit
            Destroy(gameObject);
        }
        else if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
