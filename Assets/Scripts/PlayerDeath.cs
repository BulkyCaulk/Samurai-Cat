using System.Collections;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [Tooltip("Delay before respawning at checkpoint")]
    public float waitTime = 0.04f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            StartCoroutine(HandleDeath(other));
    }

    private IEnumerator HandleDeath(Collider2D playerCollider)
    {
        // Hide the player
        playerCollider.gameObject.SetActive(false);
        yield return new WaitForSeconds(waitTime);
        GameManager.Instance.ReloadToCheckpoint();
    }
}
