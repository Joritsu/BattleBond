using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Gun Settings")]
    [Tooltip("Bullet prefab to instantiate.")]
    public GameObject bulletPrefab;
    [Tooltip("Transform representing the muzzle (where bullets spawn).")]
    public Transform muzzlePoint;
    [Tooltip("Fire rate (bullets per second).")]
    public float fireRate = 5f;  

    private float fireCooldown;

    void Update()
    {
        // Fire when the left mouse button is held down and the cooldown has elapsed.
        if (Input.GetButton("Fire1") && fireCooldown <= 0f)
        {
            Fire();
            fireCooldown = 1f / fireRate;
        }
        fireCooldown -= Time.deltaTime;
    }

    void Fire()
    {
        if (bulletPrefab == null || muzzlePoint == null)
        {
            Debug.LogWarning("Missing bulletPrefab or muzzlePoint.");
            return;
        }
        
        // Instantiate the bullet.
        GameObject bullet = Instantiate(bulletPrefab, muzzlePoint.position, muzzlePoint.rotation);
        // Get the Bullet component and set its direction.
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            // The direction is typically the forward direction of the muzzle. In 2D, that's transform.right.
            bulletScript.SetDirection(muzzlePoint.right);
        }
    }
}
