using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    private Vector3 _objectStartPosition;
    private Rigidbody2D _objectRigidbody;
    
    void Start()
    {
        _objectStartPosition = this.transform.position;
        _objectRigidbody = this.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        //changes gravity scale of object to simulate floating
        if(_objectStartPosition.y > this.transform.position.y + .04f)
        {
            _objectRigidbody.gravityScale = -.01f;
        }
        else
        {
            _objectRigidbody.gravityScale = .01f;
        }
    }



/////////////////////////////////////////////////////////////////////////////////////////////////////////
///          
/// 
/// 


// Changes the player mass because the platforms would fall
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "FloorBoxCollider")
        {
            Rigidbody2D playerRB = collision.GetComponentInParent<Rigidbody2D>();
            Debug.Log("enter");
            playerRB.mass = .002f;
        }
    }


// Reverts player mass when player leaves platform
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.name == "FloorBoxCollider")
        {
            Rigidbody2D playerRB = collision.GetComponentInParent<Rigidbody2D>();
            Debug.Log("exit");
            playerRB.mass = 1;
        }
    }
}
