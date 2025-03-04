using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // Drag your player here in the Inspector
    public float smoothSpeed = 2f;  // Adjust for smooth movement
    public float yOffset = 2f;

    // Zoom parameters
    public float zoomSpeed = 2f;         // How fast the target zoom changes with input
    public float minZoom = 2f;           // Minimum orthographic size (zoom in limit)
    public float maxZoom = 10f;          // Maximum orthographic size (zoom out limit)
    public float zoomSmoothTime = 0.2f;  // Smoothing time for zoom transitions

    private float targetZoom;          // Desired zoom level based on input
    private float zoomVelocity = 0f;   // Velocity variable used by SmoothDamp

    void Start()
    {
        if (Camera.main != null)
        {
            // Initialize targetZoom with the current orthographic size
            targetZoom = Camera.main.orthographicSize;
        }
    }

    void Update()
    {
        // Smoothly follow the player
        if (player != null)
        {
            Vector3 targetPosition = new Vector3(player.position.x, player.position.y + yOffset, -10f);
            transform.position = Vector3.Slerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }

        // Get scroll wheel input for zooming
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0 && Camera.main != null)
        {
            // Update the target zoom based on scroll input
            targetZoom -= scrollInput * zoomSpeed;
            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        }

        // Smoothly transition the camera's orthographic size towards the target zoom
        if (Camera.main != null)
        {
            Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, targetZoom, ref zoomVelocity, zoomSmoothTime);
        }
    }
}
