// DealDamageOnCollision.cs
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(WeaponStats))]
public class DealDamageOnCollision : MonoBehaviour
{
    [Header("Only hits this tag")]
    [SerializeField] string targetTag = "Enemy";

    WeaponStats stats;
    HashSet<Rigidbody2D> _hitThisContact = new HashSet<Rigidbody2D>();

    void Awake()
    {
        stats = GetComponent<WeaponStats>();
        // start disabled if you want no damage until it's explicitly enabled
        enabled = false;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // only damage enemies
        if (!col.collider.CompareTag(targetTag)) return;
        var health = col.collider.GetComponent<Health>();
        if (health == null) return;

        // dedupe multi‑collider rigs
        var rb = col.rigidbody;
        if (rb == null || _hitThisContact.Contains(rb)) return;
        _hitThisContact.Add(rb);

        // apply damage
        health.TakeDamage(stats.damage);

        // compute average contact normal
        Vector2 sum = Vector2.zero;
        foreach (var c in col.contacts)
            sum += c.normal;
        Vector2 normal = (sum / col.contactCount).normalized;

        // apply knockback + stun
        Vector2 impulse = -normal * stats.knockbackForce;
        var ec = rb.GetComponent<EnemyController>();
        if (ec != null)
            ec.ApplyKnockback(impulse, stats.knockbackStunDuration);
        else
            rb.AddForce(impulse, ForceMode2D.Impulse);
    }

    void OnCollisionExit2D(Collision2D col)
    {
        var rb = col.rigidbody;
        if (rb != null)
            _hitThisContact.Remove(rb);
    }
}
