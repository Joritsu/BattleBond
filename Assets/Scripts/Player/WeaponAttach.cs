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
    [Tooltip("The desired world scale for the weapon. Set this through the Inspector.")]
    public Vector3 desiredWorldScale;  // Do not hardcode a value in code if you want it solely set through the Inspector.

    void LateUpdate()
    {
        if (equipPoint == null)
            return;
        
        // Update position: follow the equip point with an optional offset.
        transform.position = equipPoint.position + positionOffset;
        
        // Update rotation: match the equip point's rotation plus any offset.
        transform.rotation = equipPoint.rotation * Quaternion.Euler(rotationOffset);
        
        // Update scale: apply the value as set in the Inspector.
        transform.localScale = desiredWorldScale;
    }
}
