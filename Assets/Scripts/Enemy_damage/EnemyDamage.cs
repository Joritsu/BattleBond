using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
<<<<<<< Updated upstream
    [SerializeField] private float damage;
=======
    [SerializeField] private int damage;
>>>>>>> Stashed changes

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            collision.GetComponent<Health>().TakeDamage(damage);
    }
}
