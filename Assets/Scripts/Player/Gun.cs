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
    
    // Gun will only fire if isEquipped is true.
    [Tooltip("Whether the gun is currently equipped.")]
    public bool isEquipped = false;

    private float fireCooldown;

    void Update()
    {
        // Only fire if the gun is equipped.
        if (!isEquipped)
            return;

        // Fire if the fire button is held down and the cooldown has elapsed.
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
        
        // Instantiate the bullet at the muzzle position and rotation.
        GameObject bullet = Instantiate(bulletPrefab, muzzlePoint.position, muzzlePoint.rotation);
        // Get the Bullet component and set its direction.
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            // In 2D, the forward direction is usually the right.
            bulletScript.SetDirection(muzzlePoint.right);
        }
    }

    /// <summary>
    /// Sets whether the gun is equipped.
    /// Call this from your weapon equipping system.
    /// </summary>
    /// <param name="equipped">True if equipped, false if dropped.</param>
    public void SetEquipped(bool equipped)
    {
        isEquipped = equipped;
    }
}
