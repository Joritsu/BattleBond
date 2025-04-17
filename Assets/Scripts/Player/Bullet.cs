using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int damage = 1;
    private Vector2 direction;
    public float speed = 10f;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }
    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            var health = other.GetComponent<Health>();
            if (health != null)
                health.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
