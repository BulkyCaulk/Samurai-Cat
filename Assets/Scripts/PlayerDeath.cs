using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public float waitTime = 0.04f;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            StartCoroutine(Wait(collider));
        }
    }

    IEnumerator Wait(Collider2D collider)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(collider.gameObject);
    }
}
