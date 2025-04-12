using UnityEngine;

public class Parallax : MonoBehaviour
{
    // Adjust this value in the Inspector to control the parallax effect
    [Range(0f, 1f)]
    public float parallaxFactor = 0.5f;

    private Transform cameraTransform;
    private Vector3 previousCameraPosition;

    void Start()
    {
        // Cache the main camera's transform
        cameraTransform = Camera.main.transform;
        previousCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        // Calculate how much the camera has moved since the last frame
        Vector3 deltaMovement = cameraTransform.position - previousCameraPosition;
        // Move the background by a fraction of the camera's movement
        transform.position += new Vector3(deltaMovement.x * parallaxFactor, deltaMovement.y * parallaxFactor, 0);
        // Update the previous camera position for the next frame
        previousCameraPosition = cameraTransform.position;
    }
}
