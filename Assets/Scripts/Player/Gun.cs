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
        
        // inside Gun.Fire()

        GameObject bullet = Instantiate(bulletPrefab, muzzlePoint.position, muzzlePoint.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            // read the gun’s own damage
            var stats = GetComponent<WeaponStats>();
            int dmg = stats != null ? stats.damage : 1;
            
            bulletScript.SetDirection(muzzlePoint.right);
            bulletScript.SetDamage(dmg);
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
