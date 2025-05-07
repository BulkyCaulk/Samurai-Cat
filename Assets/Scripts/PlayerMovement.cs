using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float sprintSpeed = 8f;
    private float currentSpeed;
    public float jump = 5f;
    [SerializeField] private Rigidbody2D rb;
    private float moveInput;
    public float MoveInput { get { return moveInput;} }
    private bool grounded;
    [SerializeField] private Transform groundCheck;
    public float checkRadius = 0.2f;
    private bool _facingRight = true;
    public float maxFallSpeed = 0.1f;
    public LayerMask groundLayer; // For checking if player has the dash ability at all
    public bool hasDash; // For checking if the player can dash when having the dash ability
    private bool canDash = true;
    private bool isDashing;
    public bool hasDoubleJump;
    private bool canDoubleJump = true;
    private LayerMask groundAndPlatformLayer;
    private Knockback _knockback;
    private PlayerAttack playerAttack;
    private Animator _playerAnimator;
    private Collider2D _playerSecondCollider;
    [SerializeField] private Collider2D _playerThirdCollider;
    [SerializeField] private float dashingPower = 20f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = .5f;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private LayerMask oneWayPlatformLayer;

    public delegate void OnPlayerMove();
    public event OnPlayerMove OnPlayerRun;
    public event OnPlayerMove OnPlayerNotRun;
    public event OnPlayerMove OnPlayerSprint;
    public event OnPlayerMove OnPlayerNotSprint;
    
    void Start()
    {
        groundAndPlatformLayer = groundLayer | oneWayPlatformLayer;
        hasDash = GameManager.Instance.UnlockedDash;
        hasDoubleJump = GameManager.Instance.UnlockedDoubleJump;
        //GameManager.Instance.UnlockDash();
        GameManager.Instance.UnlockDoubleJump();
        RefreshAbilities();
        rb = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();
        _knockback = GetComponent<Knockback>();
        rb.collisionDetectionMode =  CollisionDetectionMode2D.Continuous;

        currentSpeed = speed;
        playerAttack = GetComponent<PlayerAttack>();
        playerAttack.onDownwardAttackHit += ResetDoubleJump;

        SetPlayerCollider();

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)){
            GameManager.Instance.UnlockDash();
            RefreshAbilities();
        }
        

        if(_knockback.IsBeingKnockedBack)
        {
            return;
        }
        moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput > 0 || moveInput < 0)
        {
            OnPlayerRun?.Invoke();
        }
        else 
        {
            OnPlayerNotRun?.Invoke();
        }

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
            OnPlayerSprint?.Invoke();
        }
        else
        {
            currentSpeed = speed;
            OnPlayerNotSprint?.Invoke();
        }

        // Store previous grounded state
        bool previouslyGrounded = grounded;
        grounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundAndPlatformLayer);
        
        if (previouslyGrounded && !grounded && hasDoubleJump)
        {
            canDoubleJump = true;
            Debug.Log("Set canDoubleJump to true");
        }
        
        bool onPlatform = Physics2D.OverlapCircle(groundCheck.position, checkRadius, oneWayPlatformLayer);
        if (Input.GetKeyDown(KeyCode.Space) && Input.GetKey(KeyCode.S) && onPlatform)
        {
            StartCoroutine(DropThroughPlatform());
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (grounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jump);
                canDoubleJump = true;
                Debug.Log("Jump");
            }
            else if (hasDoubleJump)
            {
                if (canDoubleJump)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jump);
                    canDoubleJump = false;
                    Debug.Log("Double Jump");
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }

    private IEnumerator Dash()
    {
        // Handles the dashing of the player
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        _playerAnimator.SetTrigger("playerDash");
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

private IEnumerator DropThroughPlatform()
{
    Collider2D platformCol = Physics2D.OverlapCircle(
        groundCheck.position,
        checkRadius,
        oneWayPlatformLayer
    );
    if (platformCol == null)                   // Not on a platform
        yield break;

    // Disable contact between player and this platform
    Collider2D playerCol = GetComponent<Collider2D>();
    Physics2D.IgnoreCollision(playerCol, platformCol, true);
    Physics2D.IgnoreCollision(_playerSecondCollider, platformCol, true);
    Physics2D.IgnoreCollision(_playerThirdCollider, platformCol, true);

    yield return new WaitForFixedUpdate();
    yield return new WaitForSeconds(0.6f);
    Physics2D.IgnoreCollision(playerCol, platformCol, false);
    Physics2D.IgnoreCollision(_playerSecondCollider, platformCol, false);
    Physics2D.IgnoreCollision(_playerThirdCollider, platformCol, false);
}


    private void ResetDoubleJump()
    {
        // Enable extra jump for the player
        if (playerAttack.objectHit != null)
        {
            canDoubleJump = true;
        }
    }

    // For when the player gains a new ability, call this method to refresh the variable
    public void RefreshAbilities()
    {
        hasDash       = GameManager.Instance.UnlockedDash;
        hasDoubleJump = GameManager.Instance.UnlockedDoubleJump;
    }
    
    private void SetPlayerCollider()
    {
        Transform child = gameObject.transform.GetChild(3);
        _playerSecondCollider = child.GetComponent<BoxCollider2D>();

    }
}
