using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    [HideInInspector] public bool isStunned { get; private set; }

    Rigidbody2D rb;
    EnemyAI ai;
    EnemyPathfinder pathfinder;
    PatrolBehavior patrol;

    void Awake()
    {
        rb         = GetComponent<Rigidbody2D>();
        ai         = GetComponent<EnemyAI>();
        pathfinder = GetComponent<EnemyPathfinder>();
        patrol     = GetComponent<PatrolBehavior>();
    }

    /// <summary>
    /// Called by your weapon on hit.
    /// </summary>
    public void ApplyKnockback(Vector2 force, float stunDuration)
    {
        rb.AddForce(force, ForceMode2D.Impulse);

        // don’t stack multiple coroutines
        if (!isStunned)
            StartCoroutine(StunCoroutine(stunDuration));
    }

    IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;
        // disable all movement/AI
        if (ai         != null) ai.enabled         = false;
        if (pathfinder != null) pathfinder.enabled = false;
        if (patrol     != null) patrol.enabled     = false;

        yield return new WaitForSeconds(duration);

        // re‑enable movement/AI
        if (ai         != null) ai.enabled         = true;
        if (pathfinder != null) pathfinder.enabled = true;
        if (patrol     != null) patrol.enabled     = true;
        isStunned = false;
    }
}
