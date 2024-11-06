using UnityEngine;

public class Player2Controller : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float wallSlideSpeed = 2f;
    public float wallJumpForce = 7f;
    public float wallHopForce = 5f;
    public Vector2 wallHopDirection = new Vector2(1, 1);
    public Vector2 wallJumpDirection = new Vector2(1, 1);

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isGrounded;
    private bool isWalled;
    private bool isWallSliding;
    private bool canJump;

    private PlayerInputActions inputActions;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputActions = new PlayerInputActions();

        // Set up input action callbacks
        inputActions.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Movement.canceled += ctx => moveInput = Vector2.zero;
        inputActions.Player.Jump.performed += ctx => Jump();

        wallHopDirection.Normalize();
        wallJumpDirection.Normalize();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Update()
    {
        Move();
        CheckWallSliding();
    }

    private void Move()
    {
        if (!isWallSliding)
        {
            rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        else if (isWallSliding || isWalled)
        {
            isWallSliding = false;
            WallJump();
        }
    }

    private void WallJump()
    {
        float jumpDirection = rb.velocity.x < 0 ? 1 : -1; // Automatically flip direction
        if (moveInput.x == 0)  // Wall hop
        {
            Vector2 wallHop = new Vector2(wallHopForce * wallHopDirection.x * jumpDirection, wallHopForce * wallHopDirection.y);
            rb.velocity = wallHop;
        }
        else  // Wall jump
        {
            Vector2 wallJump = new Vector2(wallJumpForce * wallJumpDirection.x * jumpDirection, wallJumpForce * wallJumpDirection.y);
            rb.velocity = wallJump;
        }
    }

    private void CheckWallSliding()
    {
        // Check if player should start wall sliding
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, LayerMask.GetMask("Ground"));
        isWalled = Physics2D.Raycast(transform.position, Vector2.right, 0.6f, LayerMask.GetMask("Ground")) ||
                   Physics2D.Raycast(transform.position, Vector2.left, 0.6f, LayerMask.GetMask("Ground"));

        if (isWalled && !isGrounded && rb.velocity.y < 0)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
        }
        else
        {
            isWallSliding = false;
        }
    }
}
