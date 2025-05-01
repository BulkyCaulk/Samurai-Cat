using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDropped : MonoBehaviour
{
    private Rigidbody2D rb;
    public float fallspeed = -1f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 2f);
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(rb.velocity.x, fallspeed);
    }

}
