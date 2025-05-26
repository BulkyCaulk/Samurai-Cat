using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ElevatorPlatform : MonoBehaviour
{
    [Header("Initial Settings")]
    [Tooltip("If true, elevator starts moving toward the end position when scene loads")]
    [SerializeField] private bool moveUpOnStart = false;

    [Header("Movement Settings")]
    [Tooltip("Local-space offset from the start position to the end position")]
    [SerializeField] private Vector2 movementOffset = new Vector2(0f, 5f);
    [SerializeField] private float movementSpeed = 2f;
    [Tooltip("How close is 'close enough' to switch direction")]
    [SerializeField] private float arrivalThreshold = 0.01f;

    private Rigidbody2D platformRigidbody;
    private Vector2 startPosition;
    private Vector2 endPosition;

    private bool isAtStart = true;
    private bool isMoving = false;
    public bool IsMoving => isMoving;
    private Vector2 lastStepDelta = Vector2.zero;

    private void Awake()
    {
        platformRigidbody = GetComponent<Rigidbody2D>();
        platformRigidbody.isKinematic = true;

        startPosition = platformRigidbody.position;
        endPosition = startPosition + movementOffset;
    }

    private void Start()
    {
        if (moveUpOnStart)
        {
            ToggleElevator();
        }
    }
    /// Call to start the elevator moving toward its other end.

    public void ToggleElevator()
    {
        if (isMoving) return;
        StartCoroutine(MovePlatformRoutine());
    }

    private IEnumerator MovePlatformRoutine()
    {
        isMoving = true;
        Vector2 target = isAtStart ? endPosition : startPosition;

        while (Vector2.Distance(platformRigidbody.position, target) > arrivalThreshold)
        {
            Vector2 currentPos = platformRigidbody.position;
            Vector2 nextPos = Vector2.MoveTowards(currentPos, target, movementSpeed * Time.fixedDeltaTime);

            lastStepDelta = nextPos - currentPos;
            platformRigidbody.MovePosition(nextPos);

            yield return new WaitForFixedUpdate();
        }

        // Snap exactly and reset delta
        platformRigidbody.MovePosition(target);
        lastStepDelta = Vector2.zero;

        isAtStart = !isAtStart;
        isMoving = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!isMoving) return;
        if (collision.transform.CompareTag("Player") && collision.rigidbody != null)
        {
            // Carry the player smoothly by applying same delta
            collision.rigidbody.MovePosition(collision.rigidbody.position + lastStepDelta);
        }
    }
}
