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
    private PlayerMeleeAttack handAttack;

    void Start()
    {
        // 2) Find it (assumes it’s on this object or a child)
        handAttack = GetComponentInChildren<PlayerMeleeAttack>();
    }

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
// In your weapon prefab:
// 1. Add the WeaponAttachment component in the prefab.
// 2. Set desiredWorldScale in the Inspector to your chosen value (e.g. 0.75, 1, 1).
// 3. (Optional) Disable the script so it doesn't run until equipped.

// In your WeaponEquip script, do something like:

    public void EquipWeapon(GameObject weapon)
    {
        if (equippedWeapon != null)
            DropWeapon();
        
        equippedWeapon = weapon;

        // Enable/disable the script instead of adding/removing it:
        WeaponAttachment attachment = equippedWeapon.GetComponent<WeaponAttachment>();
        if (attachment != null)
        {
            attachment.enabled = true;  // Let the script run in LateUpdate
            attachment.equipPoint = equipPoint;
        }
        Gun gunComponent = equippedWeapon.GetComponent<Gun>();
        if (gunComponent != null)
            gunComponent.SetEquipped(true);

        if (handAttack != null)
            handAttack.enabled = false;
        
        // Set physics to Kinematic, etc.
    }

    // Then, dropping:
    public void DropWeapon()
    {
        if (equippedWeapon == null)
            return;
        
        // Get and disable the WeaponAttachment component.
        WeaponAttachment attachment = equippedWeapon.GetComponent<WeaponAttachment>();
        if (attachment != null)
        {
            attachment.enabled = false;
            attachment.equipPoint = null;
        }

        // Get the Gun component before clearing equippedWeapon.
        Gun gunComponent = equippedWeapon.GetComponent<Gun>();
        
        // Unparent the weapon.
        equippedWeapon.transform.SetParent(null, false);
        
        // Re-enable its physics if needed.
        Rigidbody2D weaponRb = equippedWeapon.GetComponent<Rigidbody2D>();
        if (weaponRb != null)
            weaponRb.bodyType = RigidbodyType2D.Dynamic;
        
        // Now disable the gun so it doesn't shoot.
        if (gunComponent != null)
            gunComponent.SetEquipped(false);

        Debug.Log("Weapon dropped: " + equippedWeapon.name);
        
        // Finally, clear the equippedWeapon reference.
        equippedWeapon = null;
        if (handAttack != null)
            handAttack.enabled = true;
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

    /// <summary>
    /// Returns true if the player currently has a weapon equipped.
    /// </summary>
    public bool HasWeapon()
    {
        return equippedWeapon != null;
    }
}
