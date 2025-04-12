using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 2f;
    public float jump = 5f;
    private Rigidbody2D rb;
    private float moveInput;
    private bool grounded;
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    private bool _facingRight = true;
    public float maxFallSpeed = 20f;
    public LayerMask groundLayer;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode =  CollisionDetectionMode2D.Continuous;
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
            grounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(new Vector2(0, jump), ForceMode2D.Impulse);
        }
    }

    private void FixedUpdate()
    {
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

}
