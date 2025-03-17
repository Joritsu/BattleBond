using UnityEngine;

public class FollowMouseTarget : MonoBehaviour
{
    void Update()
    {
        if (Time.timeScale == 0f)
            return;
        
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        transform.position = mousePos;
    }
}
