using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlatform : MonoBehaviour
{

    private Rigidbody2D rb;
    public float checkRadius = 0.2f;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Fall();
        }
    }

    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(2);

        rb.velocity = new Vector2(rb.velocity.x, -2f);
        
        yield return new WaitForSeconds(4);

        Destroy(gameObject);

    }
    
}
