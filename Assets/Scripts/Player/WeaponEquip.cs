using UnityEngine;

public class WeaponEquip : MonoBehaviour
{
    [Header("Equip Settings")]
    [Tooltip("The transform where the weapon should be attached (e.g., player's hand/arm equip point). Make sure its scale is (1,1,1).")]
    public Transform equipPoint;
    
    [Tooltip("The key used to pick up or drop a weapon.")]
    public KeyCode pickupDropKey = KeyCode.Q;
    
    [Tooltip("The radius within which to search for a weapon.")]
    public float pickupRadius = 1.0f;
    
    [Tooltip("Layer mask defining which objects are considered weapons.")]
    public LayerMask weaponLayer;

    // The currently equipped weapon.
    private GameObject equippedWeapon;

    void Update()
    {
        if (Input.GetKeyDown(pickupDropKey))
        {
            if (equippedWeapon != null)
            {
                Debug.Log("Dropping weapon.");
                DropWeapon();
            }
            else
            {
                Debug.Log("Attempting pickup...");
                AttemptPickup();
            }
        }
    }

    /// <summary>
    /// Uses an OverlapCircle at the equip point to detect a nearby weapon.
    /// </summary>
    void AttemptPickup()
    {
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
    /// Adds (or enables) a WeaponAttachment component so the weapon follows the equip point,
    /// then disables its physics.
    /// </summary>
    /// <param name="weapon">The weapon GameObject to equip.</param>
    public void EquipWeapon(GameObject weapon)
    {
        if (weapon == null || equipPoint == null)
        {
            Debug.LogWarning("EquipWeapon: Weapon or equipPoint is null.");
            return;
        }
        
        // Drop any weapon already equipped.
        if (equippedWeapon != null)
            DropWeapon();

        equippedWeapon = weapon;

        // Disable the weapon's physics so it's not influenced by external forces.
        Rigidbody2D weaponRb = equippedWeapon.GetComponent<Rigidbody2D>();
        if (weaponRb != null)
        {
            weaponRb.bodyType = RigidbodyType2D.Kinematic;
        }
        
        // Detach from any previous parent.
        equippedWeapon.transform.SetParent(null, false);
        
        // Add (or get) the WeaponAttachment component so that the weapon follows the equip point.
        WeaponAttachment attachment = equippedWeapon.GetComponent<WeaponAttachment>();
        if (attachment == null)
            attachment = equippedWeapon.AddComponent<WeaponAttachment>();
        
        // Set up the attachment properties.
        attachment.equipPoint = equipPoint;
        attachment.desiredWorldScale = new Vector3(0.544f, 0.544f, 0.544f);
        // Optionally adjust offsets for fine-tuning:
        // attachment.positionOffset = new Vector3(0.1f, -0.05f, 0);
        // attachment.rotationOffset = new Vector3(0, 0, 10);

        Debug.Log("Equipped weapon: " + equippedWeapon.name);
    }

    /// <summary>
    /// Drops the currently equipped weapon, unparents it, removes the WeaponAttachment, 
    /// and re-enables its dynamic physics.
    /// </summary>
    public void DropWeapon()
    {
        if (equippedWeapon == null)
            return;
        
        // Remove the WeaponAttachment component (so it no longer updates the transform).
        WeaponAttachment attachment = equippedWeapon.GetComponent<WeaponAttachment>();
        if (attachment != null)
        {
            Destroy(attachment);
        }
        
        // Unparent the weapon.
        equippedWeapon.transform.SetParent(null, false);
        
        // Re-enable its physics.
        Rigidbody2D weaponRb = equippedWeapon.GetComponent<Rigidbody2D>();
        if (weaponRb != null)
        {
            weaponRb.bodyType = RigidbodyType2D.Dynamic;
        }
        
        Debug.Log("Weapon dropped: " + equippedWeapon.name);
        equippedWeapon = null;
    }

    // Optional: Visualize the pickup radius in the Scene view.
    void OnDrawGizmosSelected()
    {
        if (equipPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(equipPoint.position, pickupRadius);
        }
    }
}
