using UnityEngine;

public class FollowMouseWithRotation : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //Kad veiktu pause checkina laika ar nesustojes
        if (Time.timeScale == 0f)
            return;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Make sure to zero out the z-coordinate for 2D calculations.
        mouseWorldPos.z = 0f;

        // Calculate the direction from the hand's current position to the mouse position.
        Vector3 direction = mouseWorldPos - transform.position;

        // Determine the angle in degrees between the x-axis and the direction vector.
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the hand to face the mouse.
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
