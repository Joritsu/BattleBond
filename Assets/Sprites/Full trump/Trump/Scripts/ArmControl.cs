using UnityEngine;

public class DynamicArmTarget : MonoBehaviour
{
    [Header("References")]
    // The camera used to convert mouse screen position to world coordinates.
    public Camera mainCamera;
    // The shoulder (first bone of the arm) used as the pivot reference.
    public Transform shoulder;

    [Header("Arm Settings")]
    // Length of the upper arm (shoulder to elbow).
    public float upperArmLength = 1f;
    // Length of the forearm (elbow to hand).
    public float forearmLength = 1f;

    [Header("IK Target Settings")]
    // Z distance from the camera to your character's plane.
    public float zDistance = 10f;
    // Speed at which the target moves (for smoothing).
    public float smoothSpeed = 10f;

    [Header("Bend Settings")]
    // Magnitude of the perpendicular offset to force bending.
    public float bendOffset = 0.3f;
    // The bending effect starts when the target is closer than this threshold.
    // (Here, we use the midpoint between the minimum and maximum reachable distances.)
    // You can tweak this by modifying the calculation below if needed.

    void Update()
    {
        // 1. Convert the mouse position to world coordinates.
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = zDistance; // Set the proper depth.
        Vector3 worldMousePos = mainCamera.ScreenToWorldPoint(mousePos);

        // 2. Calculate the direction and distance from the shoulder to the mouse.
        Vector3 shoulderPos = shoulder.position;
        Vector3 dir = (worldMousePos - shoulderPos).normalized;
        float d = Vector3.Distance(shoulderPos, worldMousePos);

        // 3. Define reachable distances.
        //    Minimum reach is the absolute difference of the bone lengths.
        //    Maximum reach is the sum of the bone lengths.
        float minReach = Mathf.Abs(upperArmLength - forearmLength);
        float maxReach = upperArmLength + forearmLength;

        // Clamp the distance so it is within what the arm can reach.
        float clampedD = Mathf.Clamp(d, minReach, maxReach);

        // 4. Compute the base target position along the direction from the shoulder.
        Vector3 targetPos = shoulderPos + dir * clampedD;

        // 5. Determine a threshold for bending.
        //    Here we use the midpoint of the reachable range.
        float thresholdDistance = (minReach + maxReach) / 2f;

        // 6. If the target is closer than the threshold, apply a perpendicular offset.
        if (clampedD < thresholdDistance)
        {
            // Calculate a blend factor that goes from 1 at minReach to 0 at thresholdDistance.
            float blend = Mathf.Clamp01((thresholdDistance - clampedD) / (thresholdDistance - minReach));
            // In 2D, get a perpendicular vector (assuming XY plane).
            Vector3 perp = new Vector3(-dir.y, dir.x, 0f);
            // Apply the offset, scaled by the blend factor.
            targetPos += perp * bendOffset * blend;
        }

        // 7. Smoothly update the IK target's position.
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothSpeed);
    }
}
