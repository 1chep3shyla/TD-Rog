using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLongMapMoving : MonoBehaviour
{
    public float moveSpeed = 10f; // Speed of camera movement
    public float leftOffsetLimit = -10f; // Left boundary limit
    public float rightOffsetLimit = 10f; // Right boundary limit

    private Vector3 dragOrigin;

    void Update()
    {
        // Left Mouse Button press
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        // If Left Mouse Button is held down
        if (Input.GetMouseButton(0))
        {
            Vector3 difference = Camera.main.ScreenToViewportPoint(dragOrigin - Input.mousePosition);
            Vector3 move = new Vector3(difference.x * moveSpeed, 0, 0);

            // Calculate new camera position
            Vector3 newPos = Camera.main.transform.position + move;

            // Clamp the position within the left and right limits
            newPos.x = Mathf.Clamp(newPos.x, leftOffsetLimit, rightOffsetLimit);

            // Set the new camera position
            Camera.main.transform.position = newPos;

            // Update drag origin
            dragOrigin = Input.mousePosition;
        }
    }
}