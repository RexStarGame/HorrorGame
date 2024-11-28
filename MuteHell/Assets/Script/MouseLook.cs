using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;  // Sensitivity for mouse movement

    public Transform playerBody;  // Reference to the player's body for rotating the entire character (yaw rotation)

    private float xRotation = 0f;  // Used to store the vertical (pitch) rotation

    void Start()
    {
        // Lock the cursor to the center of the screen and hide it.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Vertical rotation (pitch), clamped to avoid flipping over
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);  // Clamping to prevent looking too far up or down

        // Apply the vertical rotation to the camera (looking up/down)
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Apply the horizontal rotation to the player body (yaw)
        playerBody.Rotate(Vector3.up * mouseX);  // Rotate the player left/right based on mouse movement
    }
}
