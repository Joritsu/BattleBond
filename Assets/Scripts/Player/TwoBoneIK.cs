using UnityEngine;

public class TwoBoneIKCorrected : MonoBehaviour
{
    [Header("Arm Segments")]
    public Transform upperArm;
    public Transform forearm;
    public Transform hand;

    [Header("Target")]
    public Transform target;

    [Header("Arm Lengths")]
    public float upperArmLength = 1f;
    public float forearmLength = 1f;

    [Header("Elbow Limits (degrees)")]
    public float minElbowAngle = 0f;   // fully extended
    public float maxElbowAngle = 135f; // fairly bent

    void Update()
    {
        if (target == null || upperArm == null || forearm == null)
            return;
        
        // Get positions
        Vector2 shoulderPos = upperArm.position;
        Vector2 targetPos = target.position;
        
        // Calculate distance and clamp within reachable limits.
        float d = Vector2.Distance(shoulderPos, targetPos);
        float minReach = Mathf.Abs(upperArmLength - forearmLength);
        float maxReach = upperArmLength + forearmLength;
        d = Mathf.Clamp(d, minReach, maxReach);
        
        // Calculate the shoulder angle.
        float angleToTarget = Mathf.Atan2(targetPos.y - shoulderPos.y, targetPos.x - shoulderPos.x);
        float angle0 = Mathf.Acos((upperArmLength * upperArmLength + d * d - forearmLength * forearmLength) / (2f * upperArmLength * d));
        float shoulderAngle = angleToTarget - angle0;
        
        // Calculate the internal elbow angle (in radians).
        float elbowAngle = Mathf.Acos((upperArmLength * upperArmLength + forearmLength * forearmLength - d * d) /
                                      (2f * upperArmLength * forearmLength));

        // Convert elbow angle to degrees, clamp, then convert back to radians.
        float elbowAngleDeg = elbowAngle * Mathf.Rad2Deg;
        elbowAngleDeg = Mathf.Clamp(elbowAngleDeg, minElbowAngle, maxElbowAngle);
        elbowAngle = elbowAngleDeg * Mathf.Deg2Rad;
        
        // Rotate the upper arm
        upperArm.rotation = Quaternion.Euler(0f, 0f, shoulderAngle * Mathf.Rad2Deg);
        
        // Rotate the forearm
        float forearmRotation = shoulderAngle * Mathf.Rad2Deg + (180f - elbowAngleDeg);
        forearm.rotation = Quaternion.Euler(0f, 0f, forearmRotation);
        
        // Optionally update the hand position
        if (hand != null)
        {
            hand.position = forearm.position + (Vector3)(forearm.right * forearmLength);
        }
    }
}
