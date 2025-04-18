using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    [Header("Melee Settings")]
    [SerializeField] private int attackDamage = 3;
    [Tooltip("Impulse force applied to the target when hit with a punch")]
    [SerializeField] private float punchKnockback = 5f;
    [SerializeField] private string targetTag = "Enemy";

    private WeaponEquip weaponEquip;

    void Awake()
    {
        weaponEquip = GetComponentInParent<WeaponEquip>();
        if (weaponEquip == null)
            Debug.LogWarning("PlayerMeleeAttack: no WeaponEquip found in parents!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If you have a weapon equipped, skip the bare‑hand attack entirely
        if (weaponEquip != null && weaponEquip.HasWeapon())
            return;

        // Only hit things tagged "Enemy"
        if (!other.CompareTag(targetTag))
            return;

        // 1) Damage
        var health = other.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(attackDamage);
            Debug.Log($"Bare‑hand dealt {attackDamage} to {other.name}");

            // 2) Knockback
            var rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // direction from attacker → target
                Vector2 dir = (other.transform.position - transform.position).normalized;
                rb.AddForce(dir * punchKnockback, ForceMode2D.Impulse);
            }
        }
    }
}
