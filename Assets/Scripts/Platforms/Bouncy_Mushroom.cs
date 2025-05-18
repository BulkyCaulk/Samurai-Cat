using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class Bouncy_Mushroom : MonoBehaviour
{
    [Tooltip("Velocity from bounce")]
    [SerializeField] private float bounceVelocity = 12f;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player")) return;

        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = new Vector2(rb.velocity.x, bounceVelocity);

        } 

        PlayerMovement pm = other.GetComponent<PlayerMovement>();
        if (pm != null)
        {
            pm.ResetDoubleJump();
        }
    }
}
