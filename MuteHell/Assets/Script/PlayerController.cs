using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;  // Reference to the CharacterController component
    public Animator animator;               // Reference to the Animator component

    public float walkSpeed = 2f;            // Walking speed
    public float runSpeed = 6f;             // Running speed
    public float sneakSpeed = 1f;           // Sneaking speed (slower than walking)
    public float rotationSpeed = 90f;       // Speed at which the player rotates (degrees per second)
    public float gravity = -9.81f;          // Gravity force

    private Vector3 velocity;               // Stores the player's velocity
    private bool isGrounded;                // Check if the player is grounded

    void Start()
    {
        // Initialize components
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Check if the player is grounded
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;  // Small downward force to ensure we stick to the ground
        }

        // Get input for movement and rotation
        float moveZ = Input.GetAxis("Vertical");    // W/S keys
        float rotateY = Input.GetAxis("Horizontal"); // A/D keys

        // Determine if the player is sneaking by pressing Left Control
        bool isSneaking = Input.GetKey(KeyCode.LeftControl);  // Sneak when Left Control is held down

        // Rotate the player based on horizontal input (A/D keys)
        float rotation = rotateY * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotation, 0);

        // Determine if the run key (Shift) is being pressed, and don't run while sneaking
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && !isSneaking;

        // Set movement speed (run, walk, or sneak)
        float speed = isSneaking ? sneakSpeed : (isRunning ? runSpeed : walkSpeed);

        // Move the player forward/backward
        Vector3 moveDirection = transform.forward * moveZ;

        // Move the player
        controller.Move(moveDirection * speed * Time.deltaTime);

        // Update animator based on movement
        if (Mathf.Abs(moveZ) > 0.1f)
        {
            if (isSneaking)
            {
                animator.SetBool("isSneaking", true);
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", false);
                animator.SetBool("isIdle", false);
            }
            else if (isRunning)
            {
                animator.SetBool("isRunning", true);
                animator.SetBool("isWalking", false);
                animator.SetBool("isSneaking", false);
                animator.SetBool("isIdle", false);
            }
            else
            {
                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", false);
                animator.SetBool("isSneaking", false);
                animator.SetBool("isIdle", false);
            }
        }
        else
        {
            // If not moving, set to idle
            animator.SetBool("isIdle", true);
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isSneaking", false);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
