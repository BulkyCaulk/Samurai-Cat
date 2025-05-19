using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class ElevatorPlatform : MonoBehaviour
{
    public float range = 5f;
    public float speed = 2f;

    private Vector3 _startPos;
    private Rigidbody2D _rb;
    private bool _isPlayerOn = false;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.isKinematic = true;
        _startPos = transform.position;
    }

    void FixedUpdate()
    {
        float targetY = _isPlayerOn 
            ? _startPos.y + range 
            : _startPos.y;

        Vector3 targetPos = new Vector3(
            _startPos.x, 
            targetY, 
            _startPos.z
        );
        Vector3 newPos = Vector3.MoveTowards(
            transform.position, 
            targetPos, 
            speed * Time.fixedDeltaTime
        );

        _rb.MovePosition(newPos);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("FloorBoxCollider"))
        {
            _isPlayerOn = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("FloorBoxCollider"))
        {
            _isPlayerOn = false;

        }
    }
}
