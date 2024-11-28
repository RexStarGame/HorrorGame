using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform playerTransform;       // Reference to the player's transform
    public Vector3 cameraOffset = new Vector3(0, 1.8f, 0); // Offset from the player's position to the camera position

    public float mouseSensitivity = 100f;   // Mouse sensitivity for looking around
    public float pitchLimit = 45f;          // Limit for vertical rotation (up-and-down)
    public float yawLimit = 90f;            // Limit for horizontal rotation (left-right)

    private float xRotation = 0f;           // Pitch (up/down rotation)
    private float yRotation = 0f;           // Yaw (left/right rotation)

    void Start()
    {
        // Lock the cursor to the center of the screen and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        // Update the camera's position to follow the player
        FollowPlayer();

        // Handle mouse input for looking around (rotation)
        HandleMouseLook();
    }

    void HandleMouseLook()
    {
        // Get mouse input for yaw (X axis) and pitch (Y axis)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Adjust the yaw (left/right rotation), clamping it within the yaw limits
        yRotation += mouseX;
        yRotation = Mathf.Clamp(yRotation, -yawLimit, yawLimit);

        // Adjust the pitch (up/down rotation), clamping it within the pitch limits
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -pitchLimit, pitchLimit);

        // Calculate the camera rotation relative to the player
        Quaternion cameraRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        // Apply the rotation
        transform.rotation = playerTransform.rotation * cameraRotation;
    }

    void FollowPlayer()
    {
        // Set the camera's position to the player's position plus offset (adjusted for player's rotation)
        transform.position = playerTransform.position + playerTransform.rotation * cameraOffset;
    }
}
