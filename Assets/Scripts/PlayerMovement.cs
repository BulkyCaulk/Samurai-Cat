using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 2f;
    public float jump = 5f;
    [SerializeField] private Rigidbody2D rb;
    private float moveInput;
    private bool grounded;
    [SerializeField] private Transform groundCheck;
    public float checkRadius = 0.2f;
    private bool _facingRight = true;
    public float maxFallSpeed = 20f;
    public LayerMask groundLayer;
    // For checking if player has the dash ability at all
    public bool hasDash = false;
    // For checking if the player can dash when having the dash ability
    private bool canDash = true;
    public bool isDashing;
    [SerializeField] private float dashingPower = 20f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = .5f;
    [SerializeField] private TrailRenderer tr;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode =  CollisionDetectionMode2D.Continuous;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            return;
        }
        moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput > 0 && !_facingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && _facingRight)
        {
            Flip();
        }
            grounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(new Vector2(0, jump), ForceMode2D.Impulse);
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
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

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
}
