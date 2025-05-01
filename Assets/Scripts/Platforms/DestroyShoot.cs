using UnityEngine;

public class DestroyShoot : MonoBehaviour
{
    [SerializeField] float speed   = 5f;   // default speed
    [SerializeField] float lifetime = 3f;  // default lifetime

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb.velocity = new Vector2(speed, rb.velocity.y);
        Destroy(gameObject, lifetime);
    }

    public void Init(float newSpeed, float? newLifetime = null)
    {
        speed = newSpeed;
        if (newLifetime.HasValue)
            lifetime = newLifetime.Value;

        rb.velocity = new Vector2(speed, rb.velocity.y);
        Destroy(gameObject, lifetime);
    }
}
