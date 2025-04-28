using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatformLeft : MonoBehaviour
{
    public float range = 10f;
    public float speed = 1f;

    private Vector3 _startPos;
    private Rigidbody2D _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.isKinematic = true;
        _startPos = transform.position;
    }

    void FixedUpdate()
    {
        float offset = Mathf.PingPong(Time.time * speed, range);
        Vector2 target = _startPos + Vector3.left * offset;
        _rb.MovePosition(target);
    }
}
