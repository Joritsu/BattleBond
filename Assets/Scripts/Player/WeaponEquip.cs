using UnityEngine;

public class WeaponEquip : MonoBehaviour
{
    [Header("Equip Settings")]
    [Tooltip("The transform where the weapon should be attached (e.g., player's hand)")]
    public Transform equipPoint;
    
    [Tooltip("The key used to pick up or drop a weapon")]
    public KeyCode pickupDropKey = KeyCode.Q;
    
    [Tooltip("The radius within which to search for a weapon")]
    public float pickupRadius = 1.0f;
    
    [Tooltip("Layer mask defining which objects are considered weapons")]
    public LayerMask weaponLayer;

    // The currently equipped weapon
    private GameObject equippedWeapon;

    void Update()
    {
        if (Input.GetKeyDown(pickupDropKey))
        {
            // If a weapon is equipped, drop it.
            if (equippedWeapon != null)
            {
                Debug.Log("Dropping weapon.");
                DropWeapon();
            }
            // Otherwise, attempt to pick one up.
            else
            {
                Debug.Log("Attempting pickup...");
                AttemptPickup();
            }
        }
    }

    /// <summary>
    /// Searches for a nearby weapon using an OverlapCircle and equips it if found.
    /// </summary>
    void AttemptPickup()
    {
        // Draw a debug circle in the scene view.
        Collider2D hit = Physics2D.OverlapCircle(equipPoint.position, pickupRadius, weaponLayer);
        if (hit != null)
        {
            Debug.Log("Weapon found: " + hit.gameObject.name);
            EquipWeapon(hit.gameObject);
        }
        else
        {
            Debug.Log("No weapon found within pickup radius at " + equipPoint.position);
        }
    }

    /// <summary>
    /// Equips the specified weapon.
    /// </summary>
    /// <param name="weapon">The weapon GameObject to equip.</param>
    public void EquipWeapon(GameObject weapon)
    {
        if (weapon == null || equipPoint == null)
        {
            Debug.LogWarning("EquipWeapon: Weapon or equipPoint is null.");
            return;
        }
        
        // If a weapon is already equipped, drop it first.
        if (equippedWeapon != null)
            DropWeapon();

        equippedWeapon = weapon;

        // Disable the weapon's physics so it doesn't interfere with the player's movement.
        Rigidbody2D rb = equippedWeapon.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        // Store the weapon's original world scale.
        Vector3 originalWorldScale = equippedWeapon.transform.lossyScale;
        
        // Parent the weapon to the equip point while preserving its world transform.
        equippedWeapon.transform.SetParent(equipPoint, true);
        
        // Snap the weapon to the equip point.
        equippedWeapon.transform.localPosition = Vector3.zero;
        equippedWeapon.transform.localRotation = Quaternion.identity;
        
        // Recalculate the weapon's local scale so its world scale remains the same.
        Vector3 equipPointScale = equipPoint.lossyScale;
        equippedWeapon.transform.localScale = new Vector3(
            originalWorldScale.x / equipPointScale.x,
            originalWorldScale.y / equipPointScale.y,
            originalWorldScale.z / equipPointScale.z);
        
        Debug.Log("Equipped weapon: " + equippedWeapon.name);
    }

    /// <summary>
    /// Drops the currently equipped weapon.
    /// </summary>
    public void DropWeapon()
    {
        if (equippedWeapon == null)
            return;

        // Unparent the weapon so that it becomes an independent object.
        equippedWeapon.transform.SetParent(null, true);

        // Re-enable its physics.
        Rigidbody2D rb = equippedWeapon.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }

        Debug.Log("Weapon dropped: " + equippedWeapon.name);
        equippedWeapon = null;
    }

    // Optional: Visualize the pickup radius in the scene view.
    void OnDrawGizmosSelected()
    {
        if (equipPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(equipPoint.position, pickupRadius);
        }
    }
}
