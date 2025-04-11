using UnityEngine;

public class WeaponAttachment : MonoBehaviour
{
    [Header("Attachment Settings")]
    [Tooltip("The equip point on the arm where this weapon should be attached. This should be an empty GameObject with scale (1,1,1).")]
    public Transform equipPoint;
    [Tooltip("Optional positional offset relative to the equip point.")]
    public Vector3 positionOffset = Vector3.zero;
    [Tooltip("Optional rotational offset (in degrees) relative to the equip point.")]
    public Vector3 rotationOffset = Vector3.zero;
    
    [Header("Scale Settings")]
    [Tooltip("The desired world scale for the weapon (e.g., (0.544, 0.544, 0.544)).")]
    public Vector3 desiredWorldScale = new Vector3(0.544f, 0.544f, 0.544f);
    
    void LateUpdate()
    {
        if (equipPoint == null)
            return;
        
        // Match the equip point's position plus any offset.
        transform.position = equipPoint.position + positionOffset;
        
        // Match the equip point's rotation plus a rotational offset.
        transform.rotation = equipPoint.rotation * Quaternion.Euler(rotationOffset);
        
        // Since the equip point's scale is (1,1,1), the weapon's local scale is the same as its world scale.
        // Simply set the local scale to your desired value.
        transform.localScale = desiredWorldScale;
    }
}
