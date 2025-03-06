using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;  // Reference to the CharacterController component
    public Animator animator;               // Reference to the Animator component
    public Rigidbody rig;


    public float walkSpeed = 2f;            // Walking speed
    public float runSpeed = 6f;             // Running speed
    public float sneakSpeed = 1f;           // Sneaking speed (slower than walking)
    public float rotationSpeed = 90f;       // Speed at which the player rotates (degrees per second)
    public float gravity = -9.81f;          // Gravity force
    public float jumpForce = 5f; // spillerens spring kraft. 
    private float playerWhaigt; // spilleren vejer mere og falder ned hurtiger. 

    //public float currenthealth = 100f; // spillerens liv. 
    //public float maxHealth = 100f; // max health. 

    //these have to be public because i want to access them from another script
    public bool isRunning = false;                  // defined in the update function
    public bool isJumping = false;                  // defined in the update function

    private Vector3 velocity;               // Stores the player's velocity
    private bool isGrounded;                // Check if the player is grounded

    void Start()
    {
        isGrounded = false;
        playerWhaigt = 0;
        // Initialize components
        controller = GetComponent<CharacterController>(); // henter character controller fra gameobject
        animator = GetComponent<Animator>(); // henter animator fra gameobject
    }

    void Update()
    {
        // Check if the player is grounded
        isGrounded = controller.isGrounded; // isGrounded er true hvis vi er på jorden

        if (isGrounded && velocity.y < 0)  // hvis vi er på jorden og velocity.y er mindre end 0
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
        isRunning = Input.GetKey(KeyCode.LeftShift) && !isSneaking;
        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space)) // hvis vi trykker på space
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity); // beregner jump force
                isJumping = true; // isJumping er true
                animator.SetTrigger("isJumping"); // trigger animator
                animator.SetBool("IsJumping", true); // setter animator til true
            }
            else if (isJumping)  // Nulstil kun hvis vi var i luften
            {
                isJumping = false; // isJumping er false
                animator.SetBool("IsJumping", false); // setter animator til false
            }
        }

        // Set movement speed (run, walk, or sneak)
        float speed = isSneaking ? sneakSpeed : (isRunning ? runSpeed : walkSpeed); // beregner speed

        // Move the player forward/backward
        Vector3 moveDirection = transform.forward * moveZ; // beregner moveDirection

        // Move the player
        controller.Move(moveDirection * speed * Time.deltaTime); // beregner moveDirection

        // Update animator based on movement
        if (Mathf.Abs(moveZ) > 0.1f) // hvis vi bevger os 
        {
            if (isSneaking) // hvis vi er sneaking
            {
                animator.SetBool("isSneaking", true); // setter animator til true
                animator.SetBool("isWalking", false); // setter animator til false
                animator.SetBool("isRunning", false); // setter animator til false
                animator.SetBool("isIdle", false); // setter animator til false
            }
            else if (isRunning) // hvis vi er running
            {
                animator.SetBool("isRunning", true); // setter animator til true
                animator.SetBool("isWalking", false); // setter animator til false
                animator.SetBool("isSneaking", false); // setter animator til false
                animator.SetBool("isIdle", false); // setter animator til false
            }
            else
            {
                animator.SetBool("isWalking", true); // setter animator til true
                animator.SetBool("isRunning", false); // setter animator til false
                animator.SetBool("isSneaking", false); // setter animator til false
                animator.SetBool("isIdle", false); // setter animator til false
            }
  
        }
        else
        {
            // If not moving, set to idle
            animator.SetBool("isIdle", true); // setter animator til true
            animator.SetBool("isWalking", false); // setter animator til false
            animator.SetBool("isRunning", false); // setter animator til false
            animator.SetBool("isSneaking", false); // setter animator til false
            animator.SetBool("IsJumping", false); // setter animator til false
        }
         if (isJumping && isGrounded == true) // hvis vi er i luften
        {
            animator.SetTrigger("isJumping"); // trigger animator
            playerWhaigt = 5f; // tilføjer extra vægt til spilleren, spilleren falder hurtiger ned efter håb.
            isGrounded = false; // spilleren er ikke på grund.
            if(playerWhaigt > 0 )
            {
                
            }
        }
         else // tjækker spilleren om spilleren er på grund eller ej. 
        {
            isGrounded = true; // spilleren er på grund, og kan  håbbe igen.
            playerWhaigt = 0f; // spillerens vægt er reset til 0.
            animator.ResetTrigger("isJumping"); // stopper animationen. 
        }
        // Apply gravity
        velocity.y += gravity * Time.deltaTime; // beregner velocity.y
        controller.Move(velocity * Time.deltaTime); // beregner velocity    
    }
  
    public void PlayerIsDead()
    {
            GameObject.Destroy(gameObject);
            if (gameObject == null)
            {
                Time.timeScale = 0f;
                
            }   
    }
}
