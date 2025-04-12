using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Tooltip("Bullet speed (units per second)")]
    public float speed = 20f;
    
    [Tooltip("Time in seconds before the bullet is destroyed automatically.")]
    public float lifeTime = 2f;
    
    // The direction of bullet travel (set when the bullet is fired).
    private Vector2 direction;

    void Start()
    {
        // Destroy bullet after a set lifetime so it doesn't remain in the scene forever.
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Move the bullet forward.
        transform.Translate(direction * speed * Time.deltaTime);
    }

    /// <summary>
    /// Set the direction that the bullet will travel.
    /// </summary>
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        // Optionally, rotate the bullet to face the direction:
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    // Detect collisions with target objects.
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Here you can check if the bullet hit an enemy, wall, etc.
        Debug.Log("Bullet hit: " + collision.gameObject.name);
        // For now, simply destroy the bullet.
        Destroy(gameObject);
    }
}
