using UnityEngine;

public class MovementScript : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 360f;
    public float jumpSpeed = 5f;
    private float ySpeed = 0f;
    private CharacterController conn;

    private bool isGrounded; // Custom grounded check
    private float groundCheckTolerance = 0.2f; // Tolerance for ground check
    private float lastGroundedTime; // Time since the character was last grounded
    private float coyoteTime = 0.2f; // Allow jumps slightly after leaving the ground
    private float jumpBufferTime = 0.2f; // Buffer for early jump inputs
    private float jumpBuffer; // Time remaining for buffered jump

    public float dashSpeed = 20f; // Dash speed multiplier
    private float dashCooldown = 3f; // Cooldown time for dashing
    private float lastDashTime; // Time when the last dash occurred
    private bool isDashing = false; // Tracks if the player is currently dashing
    private float dashDuration = 0.5f; // How long the dash lasts
    private float dashTimer; // Tracks dash duration

    void Start()
    {
        conn = GetComponent<CharacterController>();
    }

    void Update()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");

        // Normalize the vector to ensure consistent movement speed in all directions
        Vector3 moveDirection = new Vector3(horizontalMove, 0f, verticalMove);
        moveDirection.Normalize();

        // Movement in the horizontal plane
        Vector3 horizontalVelocity = moveDirection * speed;

        // Handle dashing
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= lastDashTime + dashCooldown && moveDirection != Vector3.zero)
        {
            isDashing = true;
            lastDashTime = Time.time;
            dashTimer = dashDuration;
        }

        if (isDashing)
        {
            horizontalVelocity = moveDirection * dashSpeed; // Dash movement
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
            {
                isDashing = false;
            }
        }

        // Apply rotation to face the movement direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotate = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotate, rotationSpeed * Time.deltaTime);
        }

        // Update grounded status with tolerance
        isGrounded = conn.isGrounded || Physics.Raycast(transform.position, Vector3.down, groundCheckTolerance);

        // Update jump buffer
        if (Input.GetButtonDown("Jump"))
        {
            jumpBuffer = jumpBufferTime;
        }
        else
        {
            jumpBuffer -= Time.deltaTime;
        }

        // Apply gravity and jumping logic
        if (isGrounded)
        {
            ySpeed = -0.5f; // Slight downward force to keep the character grounded
            lastGroundedTime = Time.time; // Update grounded time
            if (jumpBuffer > 0) // Check buffered jump input
            {
                ySpeed = jumpSpeed;
                jumpBuffer = 0; // Consume jump buffer
            }
        }
        else
        {
            if (Time.time - lastGroundedTime <= coyoteTime && jumpBuffer > 0) // Coyote time logic
            {
                ySpeed = jumpSpeed;
                jumpBuffer = 0; // Consume jump buffer
            }
            else
            {
                ySpeed += Physics.gravity.y * Time.deltaTime; // Apply gravity
            }
        }

        // Combine horizontal and vertical velocities
        Vector3 velocity = horizontalVelocity;
        velocity.y = ySpeed;

        // Move the character controller
        conn.Move(velocity * Time.deltaTime);
    }
}
