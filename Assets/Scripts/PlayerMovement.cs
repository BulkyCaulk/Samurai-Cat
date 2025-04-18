using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 5f;
    public float sprintSpeed = 8f;
    private float currentSpeed;
    public float jump = 5f;
    [SerializeField] private Rigidbody2D rb;
    private float moveInput;
    private bool grounded;
    [SerializeField] private Transform groundCheck;
    public float checkRadius = 0.2f;
    private bool _facingRight = true;
    public float maxFallSpeed = 0.1f;
    public LayerMask groundLayer; // For checking if player has the dash ability at all
    public bool hasDash = false; // For checking if the player can dash when having the dash ability
    private bool canDash = true;
    private bool isDashing;
    public bool hasDoubleJump = false;
    private bool canDoubleJump = true;
    [SerializeField] private float dashingPower = 20f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = .5f;
    [SerializeField] private TrailRenderer tr;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode =  CollisionDetectionMode2D.Continuous;

        currentSpeed = speed;
        var playerAttack = GetComponent<PlayerAttack>();
        if (playerAttack != null)
        {
            playerAttack.onDownwardAttackAnimation += ResetDoubleJump;
        }
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput > 0 && !_facingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && _facingRight)
        {
            Flip();
        }
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = speed;
        }

        // Store previous grounded state
        bool previouslyGrounded = grounded;
        grounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        
        if (previouslyGrounded && !grounded && hasDoubleJump)
        {
            canDoubleJump = true;
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (grounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jump);
                canDoubleJump = true;
            }
            else if (hasDoubleJump)
            {
                if (canDoubleJump)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jump);
                    canDoubleJump = false;
                }
            }        
        }
        if (hasDash)
        {
            if (Input.GetKeyDown(KeyCode.L) && canDash)
            {
                StartCoroutine(Dash());
            }
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        rb.velocity = new Vector2(moveInput * currentSpeed, rb.velocity.y);

        if (rb.velocity.y < -maxFallSpeed)
        {
            rb.velocity = new Vector2(moveInput * currentSpeed, -maxFallSpeed);
        }

    }

    private void Flip()
    {
        _facingRight = !_facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        // Draw a red wireframe sphere at the groundCheck position
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }

    private IEnumerator Dash()
    {
        // Handles the dashing of the player
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);

        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;

    }

    private void ResetDoubleJump()
    {
        // Enable extra jump for the player
        canDoubleJump = true;
    }
}
